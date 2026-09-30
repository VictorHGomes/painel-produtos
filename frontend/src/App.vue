<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header class="cabecalho">
    <div class="cabecalho-conteudo">
      <span class="cabecalho-titulo">Painel de Manutenção de Produtos</span>
      <nav class="cabecalho-nav">
        <RouterLink to="/produtos">Produtos</RouterLink>
      </nav>
    </div>
  </header>

  <main class="conteudo">
    <RouterView />
  </main>
</template>

<style scoped>
.cabecalho {
  border-bottom: 1px solid var(--color-border);
  margin-bottom: 1.5rem;
}

.cabecalho-conteudo {
  max-width: 1100px;
  margin: 0 auto;
  padding: 1rem 1.5rem;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.cabecalho-titulo {
  font-weight: bold;
  font-size: 1.1rem;
}

.cabecalho-nav {
  display: flex;
  gap: 1rem;
}

.cabecalho-nav a {
  text-decoration: none;
}

.cabecalho-nav a.router-link-active {
  font-weight: bold;
}

.conteudo {
  max-width: 1100px;
  margin: 0 auto;
  padding: 0 1.5rem 2rem;
}
</style>
