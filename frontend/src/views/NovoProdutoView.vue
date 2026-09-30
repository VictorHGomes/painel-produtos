<script setup>
import { reactive, ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { criarProduto } from '@/services/produtosService'
import { listarCategorias } from '@/services/categoriasService'

const router = useRouter()

const form = reactive({
  nome: '',
  descricao: '',
  preco: '',
  estoque: '',
  categoriaId: '',
})

const categorias = ref([])
const erros = ref({})
const erroApi = ref('')
const salvando = ref(false)

const camposConhecidos = ['nome', 'descricao', 'preco', 'estoque', 'categoriaId']

function validar() {
  const novosErros = {}
  const nome = form.nome.trim()

  if (nome.length < 2 || nome.length > 150) {
    novosErros.nome = 'O nome deve ter entre 2 e 150 caracteres.'
  }

  if (form.descricao && form.descricao.length > 500) {
    novosErros.descricao = 'A descrição deve ter no máximo 500 caracteres.'
  }

  if (!form.preco || form.preco <= 0) {
    novosErros.preco = 'O preço deve ser maior que zero.'
  }

  if (form.estoque === '' || form.estoque < 0) {
    novosErros.estoque = 'O estoque não pode ser negativo.'
  }

  if (!form.categoriaId) {
    novosErros.categoriaId = 'Selecione uma categoria.'
  }

  erros.value = novosErros
  return Object.keys(novosErros).length === 0
}

// a API pode devolver as chaves de erro em camelCase ou PascalCase
function mapearErrosApi(errosApi) {
  const mapa = {}
  for (const chave of Object.keys(errosApi)) {
    const campo = camposConhecidos.find((c) => c.toLowerCase() === chave.toLowerCase())
    if (campo) mapa[campo] = errosApi[chave][0]
  }
  return mapa
}

async function salvar() {
  erroApi.value = ''

  if (!validar()) return

  salvando.value = true

  try {
    const produtoCriado = await criarProduto({
      nome: form.nome.trim(),
      descricao: form.descricao ? form.descricao.trim() : null,
      preco: form.preco,
      estoque: form.estoque,
      categoriaId: form.categoriaId,
    })
    router.push(`/produtos/${produtoCriado.id}`)
  } catch (erroRequisicao) {
    if (erroRequisicao.response?.status === 400) {
      erros.value = mapearErrosApi(erroRequisicao.response.data.errors)
    } else {
      erroApi.value = 'Não foi possível salvar o produto. Tente novamente.'
    }
  } finally {
    salvando.value = false
  }
}

onMounted(async () => {
  try {
    categorias.value = await listarCategorias()
  } catch {
    categorias.value = []
  }
})
</script>

<template>
  <section class="pagina-formulario">
    <div class="card formulario-card">
      <h1>Novo produto</h1>
      <p class="subtitulo">Preencha os dados abaixo para cadastrar um produto.</p>

      <div v-if="erroApi" class="alerta alerta-perigo">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z" />
          <line x1="12" y1="9" x2="12" y2="13" />
          <line x1="12" y1="17" x2="12.01" y2="17" />
        </svg>
        <p>{{ erroApi }}</p>
      </div>

      <form class="formulario" @submit.prevent="salvar">
        <div class="campo">
          <label for="nome">Nome <span class="obrigatorio">*</span></label>
          <input
            id="nome"
            v-model="form.nome"
            type="text"
            placeholder="Ex.: Cadeira de Escritório Ergonômica"
            :class="{ 'campo-erro': erros.nome }"
          />
          <p v-if="erros.nome" class="mensagem-erro">{{ erros.nome }}</p>
        </div>

        <div class="campo">
          <label for="descricao">Descrição</label>
          <textarea
            id="descricao"
            v-model="form.descricao"
            rows="3"
            placeholder="Detalhes adicionais sobre o produto"
            :class="{ 'campo-erro': erros.descricao }"
          ></textarea>
          <div class="campo-rodape">
            <p v-if="erros.descricao" class="mensagem-erro">{{ erros.descricao }}</p>
            <span class="contador" :class="{ 'contador-excedido': form.descricao.length > 500 }">
              {{ form.descricao.length }}/500
            </span>
          </div>
        </div>

        <div class="grade-dupla">
          <div class="campo">
            <label for="preco">Preço <span class="obrigatorio">*</span></label>
            <input
              id="preco"
              v-model.number="form.preco"
              type="number"
              step="0.01"
              min="0"
              placeholder="0,00"
              class="numerico"
              :class="{ 'campo-erro': erros.preco }"
            />
            <p v-if="erros.preco" class="mensagem-erro">{{ erros.preco }}</p>
          </div>

          <div class="campo">
            <label for="estoque">Estoque <span class="obrigatorio">*</span></label>
            <input
              id="estoque"
              v-model.number="form.estoque"
              type="number"
              step="1"
              min="0"
              placeholder="0"
              class="numerico"
              :class="{ 'campo-erro': erros.estoque }"
            />
            <p v-if="erros.estoque" class="mensagem-erro">{{ erros.estoque }}</p>
          </div>
        </div>

        <div class="campo">
          <label for="categoria">Categoria <span class="obrigatorio">*</span></label>
          <select
            id="categoria"
            v-model="form.categoriaId"
            :class="{ 'campo-erro': erros.categoriaId }"
          >
            <option value="">Selecione...</option>
            <option v-for="categoria in categorias" :key="categoria.id" :value="categoria.id">
              {{ categoria.nome }}
            </option>
          </select>
          <p v-if="erros.categoriaId" class="mensagem-erro">{{ erros.categoriaId }}</p>
        </div>

        <p class="nota-obrigatorio">* Campos obrigatórios</p>

        <div class="acoes">
          <button type="button" class="btn" @click="router.push('/produtos')">Cancelar</button>
          <button type="submit" class="btn btn-primario" :disabled="salvando">
            {{ salvando ? 'Salvando...' : 'Salvar' }}
          </button>
        </div>
      </form>
    </div>
  </section>
</template>

<style scoped>
.pagina-formulario {
  display: flex;
  justify-content: center;
}

.formulario-card {
  width: 100%;
  max-width: 640px;
  padding: var(--space-6);
}

.subtitulo {
  color: var(--color-text-secondary);
  font-size: 0.9rem;
  margin-top: var(--space-1);
  margin-bottom: var(--space-5);
}

.alerta {
  margin-bottom: var(--space-5);
}

.formulario {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
}

.campo {
  display: flex;
  flex-direction: column;
}

.obrigatorio {
  color: var(--color-danger);
}

.grade-dupla {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: var(--space-4);
}

.campo-rodape {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--space-3);
  margin-top: 0.3rem;
}

.campo-rodape .mensagem-erro {
  margin-top: 0;
}

.contador {
  font-size: 0.8rem;
  color: var(--color-text-secondary);
  white-space: nowrap;
  margin-left: auto;
}

.contador-excedido {
  color: var(--color-danger);
}

.nota-obrigatorio {
  font-size: 0.8rem;
  color: var(--color-text-secondary);
}

.acoes {
  display: flex;
  justify-content: flex-end;
  gap: var(--space-3);
  margin-top: var(--space-2);
}

@media (max-width: 560px) {
  .formulario-card {
    padding: var(--space-4);
  }

  .grade-dupla {
    grid-template-columns: 1fr;
  }

  .acoes {
    flex-direction: column-reverse;
  }

  .acoes .btn {
    width: 100%;
  }
}
</style>
