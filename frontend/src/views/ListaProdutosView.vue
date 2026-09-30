<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { listarProdutos } from '@/services/produtosService'
import { listarCategorias } from '@/services/categoriasService'
import PaginacaoProdutos from '@/components/PaginacaoProdutos.vue'

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
    <div class="topo">
      <h1>Produtos</h1>
      <RouterLink to="/produtos/novo" class="botao botao-primario">Novo produto</RouterLink>
    </div>

    <form class="filtros" @submit.prevent="buscar">
      <input v-model="nomeFiltro" type="text" placeholder="Buscar por nome" @keyup.enter="buscar" />

      <select v-model="categoriaFiltro">
        <option value="">Todas as categorias</option>
        <option v-for="categoria in categorias" :key="categoria.id" :value="categoria.id">
          {{ categoria.nome }}
        </option>
      </select>

      <button type="submit" class="botao">Buscar</button>
    </form>

    <p v-if="carregando">Carregando produtos...</p>
    <p v-else-if="erro">Não foi possível conectar à API. Verifique se o servidor está no ar.</p>
    <p v-else-if="produtos.length === 0">Nenhum produto encontrado.</p>

    <table v-else class="tabela-produtos">
      <thead>
        <tr>
          <th>Nome</th>
          <th>Categoria</th>
          <th>Preço</th>
          <th>Estoque</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="produto in produtos" :key="produto.id" @click="abrirDetalhe(produto.id)">
          <td>{{ produto.nome }}</td>
          <td>{{ produto.categoriaNome }}</td>
          <td>{{ formatadorPreco.format(produto.preco) }}</td>
          <td>{{ produto.estoque }}</td>
          <td>{{ produto.ativo ? 'Ativo' : 'Inativo' }}</td>
        </tr>
      </tbody>
    </table>

    <PaginacaoProdutos
      v-if="!carregando && !erro && produtos.length > 0"
      :pagina="pagina"
      :total-paginas="totalPaginas"
      :total-itens="totalItens"
      @anterior="paginaAnterior"
      @proxima="proximaPagina"
    />
  </section>
</template>

<style scoped>
.topo {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.filtros {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-bottom: 1.5rem;
}

.filtros input[type='text'] {
  flex: 1;
  min-width: 200px;
}

.tabela-produtos {
  width: 100%;
  border-collapse: collapse;
}

.tabela-produtos th,
.tabela-produtos td {
  text-align: left;
  padding: 0.6rem 0.75rem;
  border-bottom: 1px solid var(--color-border);
}

.tabela-produtos tbody tr {
  cursor: pointer;
}

.tabela-produtos tbody tr:hover {
  background-color: var(--color-background-soft);
}
</style>
