<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter, RouterLink } from 'vue-router'
import { obterProduto } from '@/services/produtosService'
import BadgeStatus from '@/components/BadgeStatus.vue'
import EstoqueDestaque from '@/components/EstoqueDestaque.vue'

const route = useRoute()
const router = useRouter()

const produto = ref(null)
const carregando = ref(true)
const naoEncontrado = ref(false)
const erro = ref(false)

const formatadorPreco = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
const formatadorData = new Intl.DateTimeFormat('pt-BR', { dateStyle: 'short', timeStyle: 'short' })

async function carregar() {
  carregando.value = true
  naoEncontrado.value = false
  erro.value = false

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
}

onMounted(carregar)
</script>

<template>
  <section class="pagina-detalhe">
    <button class="btn btn-discreto botao-voltar" @click="router.push('/produtos')">
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <line x1="19" y1="12" x2="5" y2="12" />
        <polyline points="12 19 5 12 12 5" />
      </svg>
      Voltar
    </button>

    <div v-if="carregando" class="card detalhe-card">
      <p class="texto-carregando">Carregando produto...</p>
    </div>

    <div v-else-if="naoEncontrado" class="card estado-vazio">
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
        <circle cx="11" cy="11" r="7" />
        <line x1="21" y1="21" x2="16.65" y2="16.65" />
        <line x1="8" y1="11" x2="14" y2="11" />
      </svg>
      <p>Produto não encontrado.</p>
      <RouterLink to="/produtos" class="btn btn-primario">Voltar para a listagem</RouterLink>
    </div>

    <div v-else-if="erro" class="alerta alerta-perigo">
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z" />
        <line x1="12" y1="9" x2="12" y2="13" />
        <line x1="12" y1="17" x2="12.01" y2="17" />
      </svg>
      <div>
        <p>Não foi possível conectar à API. Verifique se o servidor está no ar.</p>
        <button class="btn btn-discreto" @click="carregar">Tentar novamente</button>
      </div>
    </div>

    <div v-else class="card detalhe-card">
      <div class="detalhe-cabecalho">
        <h1>{{ produto.nome }}</h1>
        <BadgeStatus :ativo="produto.ativo" />
      </div>

      <dl class="detalhe-grade">
        <div class="detalhe-item">
          <dt>Categoria</dt>
          <dd>{{ produto.categoriaNome }}</dd>
        </div>

        <div class="detalhe-item">
          <dt>Preço</dt>
          <dd class="detalhe-preco numerico">{{ formatadorPreco.format(produto.preco) }}</dd>
        </div>

        <div class="detalhe-item">
          <dt>Estoque</dt>
          <dd><EstoqueDestaque :estoque="produto.estoque" /></dd>
        </div>

        <div class="detalhe-item detalhe-item-full">
          <dt>Descrição</dt>
          <dd>{{ produto.descricao || '-' }}</dd>
        </div>

        <div class="detalhe-item">
          <dt>Criado em</dt>
          <dd>{{ formatadorData.format(new Date(produto.dataCriacao)) }}</dd>
        </div>

        <div class="detalhe-item">
          <dt>Atualizado em</dt>
          <dd>
            {{
              produto.dataAtualizacao
                ? formatadorData.format(new Date(produto.dataAtualizacao))
                : '-'
            }}
          </dd>
        </div>
      </dl>
    </div>
  </section>
</template>

<style scoped>
.pagina-detalhe {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
  max-width: 680px;
  margin: 0 auto;
}

.botao-voltar {
  align-self: flex-start;
}

.detalhe-card {
  padding: var(--space-6);
}

.texto-carregando {
  color: var(--color-text-secondary);
  text-align: center;
  padding: var(--space-4) 0;
}

.detalhe-cabecalho {
  display: flex;
  align-items: center;
  gap: var(--space-3);
  flex-wrap: wrap;
}

.detalhe-grade {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: var(--space-5) var(--space-6);
  margin: var(--space-5) 0 0;
}

.detalhe-item dt {
  font-size: 0.72rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: var(--color-text-secondary);
  margin-bottom: 0.3rem;
}

.detalhe-item dd {
  margin: 0;
}

.detalhe-item-full {
  grid-column: 1 / -1;
}

.detalhe-preco {
  font-size: 1.4rem;
  font-weight: 700;
}

@media (max-width: 560px) {
  .detalhe-card {
    padding: var(--space-4);
  }

  .detalhe-grade {
    grid-template-columns: 1fr;
  }
}
</style>
