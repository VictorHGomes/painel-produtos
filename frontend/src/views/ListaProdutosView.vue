<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { listarProdutos } from '@/services/produtosService'
import { listarCategorias } from '@/services/categoriasService'
import PaginacaoProdutos from '@/components/PaginacaoProdutos.vue'
import BadgeStatus from '@/components/BadgeStatus.vue'
import EstoqueDestaque from '@/components/EstoqueDestaque.vue'

const router = useRouter()

const produtos = ref([])
const categorias = ref([])
const carregando = ref(false)
const erro = ref(false)

const nomeFiltro = ref('')
const categoriaFiltro = ref('')

const pagina = ref(1)
const tamanhoPagina = 10
const totalPaginas = ref(1)
const totalItens = ref(0)

const formatadorPreco = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })

const temFiltroAtivo = computed(() => nomeFiltro.value !== '' || categoriaFiltro.value !== '')

const subtitulo = computed(() => {
  if (erro.value) return ''
  if (carregando.value) return 'Carregando...'
  return totalItens.value === 1 ? '1 produto encontrado' : `${totalItens.value} produtos encontrados`
})

async function carregarProdutos() {
  carregando.value = true
  erro.value = false

  try {
    const filtros = { pagina: pagina.value, tamanhoPagina }
    if (nomeFiltro.value) filtros.nome = nomeFiltro.value
    if (categoriaFiltro.value) filtros.categoriaId = categoriaFiltro.value

    const resultado = await listarProdutos(filtros)
    produtos.value = resultado.itens
    totalPaginas.value = resultado.totalPaginas
    totalItens.value = resultado.totalItens
  } catch {
    erro.value = true
  } finally {
    carregando.value = false
  }
}

async function carregarCategorias() {
  try {
    categorias.value = await listarCategorias()
  } catch {
    categorias.value = []
  }
}

function buscar() {
  pagina.value = 1
  carregarProdutos()
}

function limparFiltros() {
  nomeFiltro.value = ''
  categoriaFiltro.value = ''
  buscar()
}

function paginaAnterior() {
  pagina.value -= 1
  carregarProdutos()
}

function proximaPagina() {
  pagina.value += 1
  carregarProdutos()
}

function abrirDetalhe(id) {
  router.push(`/produtos/${id}`)
}

onMounted(() => {
  carregarCategorias()
  carregarProdutos()
})
</script>

<template>
  <section>
    <div class="cabecalho-pagina">
      <div>
        <h1>Produtos</h1>
        <p class="subtitulo">{{ subtitulo }}</p>
      </div>

      <RouterLink to="/produtos/novo" class="btn btn-primario">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round">
          <line x1="12" y1="5" x2="12" y2="19" />
          <line x1="5" y1="12" x2="19" y2="12" />
        </svg>
        Novo produto
      </RouterLink>
    </div>

    <div class="card filtros-card">
      <form class="filtros" @submit.prevent="buscar">
        <div class="campo-busca">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round">
            <circle cx="11" cy="11" r="7" />
            <line x1="21" y1="21" x2="16.65" y2="16.65" />
          </svg>
          <input
            v-model="nomeFiltro"
            type="text"
            placeholder="Buscar por nome"
            @keyup.enter="buscar"
          />
        </div>

        <select v-model="categoriaFiltro">
          <option value="">Todas as categorias</option>
          <option v-for="categoria in categorias" :key="categoria.id" :value="categoria.id">
            {{ categoria.nome }}
          </option>
        </select>

        <button type="submit" class="btn btn-primario">Buscar</button>
        <button
          v-if="temFiltroAtivo"
          type="button"
          class="btn btn-discreto"
          @click="limparFiltros"
        >
          Limpar filtros
        </button>
      </form>
    </div>

    <div class="card tabela-card">
      <table v-if="carregando" class="tabela-produtos">
        <thead>
          <tr>
            <th class="coluna-nome">Nome</th>
            <th class="coluna-categoria">Categoria</th>
            <th class="coluna-preco col-direita">Preço</th>
            <th class="coluna-estoque col-centro">Estoque</th>
            <th class="coluna-status col-centro">Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="n in 5" :key="n">
            <td><div class="skeleton" style="width: 70%; height: 14px"></div></td>
            <td><div class="skeleton" style="width: 50%; height: 14px"></div></td>
            <td class="col-direita">
              <div class="skeleton" style="width: 64px; height: 14px; margin-left: auto"></div>
            </td>
            <td class="col-centro">
              <div class="skeleton" style="width: 32px; height: 14px; margin: 0 auto"></div>
            </td>
            <td class="col-centro">
              <div class="skeleton" style="width: 60px; height: 20px; border-radius: 999px; margin: 0 auto"></div>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-else-if="erro" class="alerta alerta-perigo">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z" />
          <line x1="12" y1="9" x2="12" y2="13" />
          <line x1="12" y1="17" x2="12.01" y2="17" />
        </svg>
        <div>
          <p>Não foi possível conectar à API. Verifique se o servidor está no ar.</p>
          <button class="btn btn-discreto" @click="carregarProdutos">Tentar novamente</button>
        </div>
      </div>

      <div v-else-if="produtos.length === 0" class="estado-vazio">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
          <path d="M3 9l1.5-5h15L21 9" />
          <path d="M3 9h18v10a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V9z" />
          <path d="M9 13h6" />
        </svg>
        <p>Nenhum produto encontrado.</p>
        <p v-if="temFiltroAtivo">Tente limpar os filtros aplicados.</p>
        <button v-if="temFiltroAtivo" class="btn btn-discreto" @click="limparFiltros">
          Limpar filtros
        </button>
      </div>

      <template v-else>
        <table class="tabela-produtos">
          <thead>
            <tr>
              <th class="coluna-nome">Nome</th>
              <th class="coluna-categoria">Categoria</th>
              <th class="coluna-preco col-direita">Preço</th>
              <th class="coluna-estoque col-centro">Estoque</th>
              <th class="coluna-status col-centro">Status</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="produto in produtos" :key="produto.id" @click="abrirDetalhe(produto.id)">
              <td data-label="Nome">
                <RouterLink :to="`/produtos/${produto.id}`" class="nome-produto" @click.stop>
                  {{ produto.nome }}
                </RouterLink>
                <p v-if="produto.descricao" class="descricao-produto">{{ produto.descricao }}</p>
              </td>
              <td data-label="Categoria">{{ produto.categoriaNome }}</td>
              <td data-label="Preço" class="col-direita numerico">
                {{ formatadorPreco.format(produto.preco) }}
              </td>
              <td data-label="Estoque" class="col-centro">
                <EstoqueDestaque :estoque="produto.estoque" />
              </td>
              <td data-label="Status" class="col-centro"><BadgeStatus :ativo="produto.ativo" /></td>
            </tr>
          </tbody>
        </table>

        <PaginacaoProdutos
          :pagina="pagina"
          :total-paginas="totalPaginas"
          :total-itens="totalItens"
          @anterior="paginaAnterior"
          @proxima="proximaPagina"
        />
      </template>
    </div>
  </section>
</template>

<style scoped>
.cabecalho-pagina {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: var(--space-3);
  margin-bottom: var(--space-5);
}

.subtitulo {
  color: var(--color-text-secondary);
  font-size: 0.9rem;
  margin-top: var(--space-1);
}

.filtros-card {
  padding: var(--space-4);
  margin-bottom: var(--space-5);
}

.filtros {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: var(--space-3);
}

.campo-busca {
  position: relative;
  flex: 1;
  min-width: 220px;
}

.campo-busca svg {
  position: absolute;
  left: 0.7rem;
  top: 50%;
  transform: translateY(-50%);
  width: 16px;
  height: 16px;
  color: var(--color-text-secondary);
  pointer-events: none;
}

.campo-busca input {
  padding-left: 2.2rem;
}

.filtros select {
  width: auto;
  min-width: 180px;
}

.tabela-card {
  overflow: hidden;
}

.tabela-produtos {
  width: 100%;
  border-collapse: collapse;
  table-layout: fixed;
}

.coluna-nome {
  width: 34%;
}

.coluna-categoria {
  width: 20%;
}

.coluna-preco {
  width: 16%;
}

.coluna-estoque {
  width: 15%;
}

.coluna-status {
  width: 15%;
}

.tabela-produtos th {
  text-align: left;
  padding: var(--space-3) var(--space-4);
  font-size: 0.72rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: var(--color-text-secondary);
  border-bottom: 1px solid var(--color-border);
}

.tabela-produtos td {
  padding: var(--space-3) var(--space-4);
  border-bottom: 1px solid var(--color-border);
  vertical-align: middle;
}

/* preco fica alinhado a direita; th e td usam o mesmo padding horizontal acima,
   entao o fim do titulo cai na mesma linha vertical do fim dos valores */
.tabela-produtos th.col-direita,
.tabela-produtos td.col-direita {
  text-align: right;
}

/* estoque e status ficam centralizados, titulo e conteudo */
.tabela-produtos th.col-centro,
.tabela-produtos td.col-centro {
  text-align: center;
}

.tabela-produtos tbody tr {
  cursor: pointer;
  transition: background-color 150ms;
}

.tabela-produtos tbody tr:hover {
  background-color: var(--color-bg);
}

.tabela-produtos tbody tr:last-child td {
  border-bottom: none;
}

.nome-produto {
  font-weight: 600;
  text-decoration: none;
  color: var(--color-text);
}

.nome-produto:hover {
  color: var(--color-primary);
}

.descricao-produto {
  margin-top: 0.2rem;
  font-size: 0.8rem;
  color: var(--color-text-secondary);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

@media (max-width: 720px) {
  .tabela-produtos thead {
    display: none;
  }

  .tabela-produtos,
  .tabela-produtos tbody,
  .tabela-produtos tr,
  .tabela-produtos td {
    display: block;
    width: 100%;
  }

  .tabela-produtos tr {
    padding: var(--space-3) var(--space-4);
  }

  .tabela-produtos td {
    border-bottom: none;
    padding: 0.3rem 0;
    text-align: left !important;
  }

  .tabela-produtos td::before {
    content: attr(data-label);
    display: block;
    font-size: 0.7rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.04em;
    color: var(--color-text-secondary);
    margin-bottom: 0.15rem;
  }

  .descricao-produto {
    white-space: normal;
  }
}
</style>
