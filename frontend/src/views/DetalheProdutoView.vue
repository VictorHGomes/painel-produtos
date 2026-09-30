<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { obterProduto } from '@/services/produtosService'

const route = useRoute()
const router = useRouter()

const produto = ref(null)
const carregando = ref(true)
const naoEncontrado = ref(false)
const erro = ref(false)

const formatadorPreco = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
const formatadorData = new Intl.DateTimeFormat('pt-BR', { dateStyle: 'short', timeStyle: 'short' })

onMounted(async () => {
  try {
    produto.value = await obterProduto(route.params.id)
  } catch (erroRequisicao) {
    if (erroRequisicao.response?.status === 404) {
      naoEncontrado.value = true
    } else {
      erro.value = true
    }
  } finally {
    carregando.value = false
  }
})
</script>

<template>
  <section>
    <button class="botao" @click="router.push('/produtos')">Voltar</button>

    <p v-if="carregando">Carregando produto...</p>
    <p v-else-if="naoEncontrado">Produto não encontrado.</p>
    <p v-else-if="erro">Não foi possível conectar à API. Verifique se o servidor está no ar.</p>

    <div v-else class="detalhe">
      <h1>{{ produto.nome }}</h1>

      <dl>
        <dt>Descrição</dt>
        <dd>{{ produto.descricao || '-' }}</dd>

        <dt>Categoria</dt>
        <dd>{{ produto.categoriaNome }}</dd>

        <dt>Preço</dt>
        <dd>{{ formatadorPreco.format(produto.preco) }}</dd>

        <dt>Estoque</dt>
        <dd>{{ produto.estoque }}</dd>

        <dt>Status</dt>
        <dd>{{ produto.ativo ? 'Ativo' : 'Inativo' }}</dd>

        <dt>Criado em</dt>
        <dd>{{ formatadorData.format(new Date(produto.dataCriacao)) }}</dd>

        <dt>Atualizado em</dt>
        <dd>
          {{
            produto.dataAtualizacao
              ? formatadorData.format(new Date(produto.dataAtualizacao))
              : '-'
          }}
        </dd>
      </dl>
    </div>
  </section>
</template>

<style scoped>
.detalhe {
  margin-top: 1.5rem;
  max-width: 480px;
}

dl {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 0.5rem 1rem;
  margin-top: 1rem;
}

dt {
  font-weight: bold;
  color: var(--color-heading);
}

dd {
  margin: 0;
}
</style>
