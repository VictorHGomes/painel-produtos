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
  <section>
    <h1>Novo produto</h1>

    <form class="formulario" @submit.prevent="salvar">
      <div class="campo">
        <label for="nome">Nome</label>
        <input
          id="nome"
          v-model="form.nome"
          type="text"
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
          :class="{ 'campo-erro': erros.descricao }"
        ></textarea>
        <p v-if="erros.descricao" class="mensagem-erro">{{ erros.descricao }}</p>
      </div>

      <div class="campo">
        <label for="preco">Preço</label>
        <input
          id="preco"
          v-model.number="form.preco"
          type="number"
          step="0.01"
          min="0"
          :class="{ 'campo-erro': erros.preco }"
        />
        <p v-if="erros.preco" class="mensagem-erro">{{ erros.preco }}</p>
      </div>

      <div class="campo">
        <label for="estoque">Estoque</label>
        <input
          id="estoque"
          v-model.number="form.estoque"
          type="number"
          step="1"
          min="0"
          :class="{ 'campo-erro': erros.estoque }"
        />
        <p v-if="erros.estoque" class="mensagem-erro">{{ erros.estoque }}</p>
      </div>

      <div class="campo">
        <label for="categoria">Categoria</label>
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

      <p v-if="erroApi" class="mensagem-erro">{{ erroApi }}</p>

      <div class="acoes">
        <button type="button" class="botao" @click="router.push('/produtos')">Cancelar</button>
        <button type="submit" class="botao botao-primario" :disabled="salvando">
          {{ salvando ? 'Salvando...' : 'Salvar' }}
        </button>
      </div>
    </form>
  </section>
</template>

<style scoped>
.formulario {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  max-width: 480px;
}

.campo {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.campo input,
.campo select,
.campo textarea {
  width: 100%;
}

.acoes {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.5rem;
}
</style>
