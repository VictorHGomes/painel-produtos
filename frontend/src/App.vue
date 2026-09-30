<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header class="cabecalho">
    <div class="cabecalho-conteudo">
      <RouterLink to="/produtos" class="cabecalho-marca">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.75" stroke-linecap="round" stroke-linejoin="round">
          <path d="M21 8l-9-5-9 5v8l9 5 9-5V8z" />
          <path d="M3 8l9 5 9-5" />
          <path d="M12 13v8" />
        </svg>
        <span>Painel de Manutenção de Produtos</span>
      </RouterLink>
    </div>
  </header>

  <main class="conteudo">
    <RouterView />
  </main>
</template>

<style scoped>
.cabecalho {
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
}

.cabecalho-conteudo {
  max-width: 1100px;
  margin: 0 auto;
  padding: var(--space-4) var(--space-5);
}

.cabecalho-marca {
  display: inline-flex;
  align-items: center;
  gap: var(--space-2);
  font-weight: 600;
  font-size: 1.05rem;
  text-decoration: none;
  color: var(--color-text);
}

.cabecalho-marca svg {
  width: 22px;
  height: 22px;
  color: var(--color-primary);
  flex-shrink: 0;
}

.conteudo {
  max-width: 1100px;
  margin: 0 auto;
  padding: var(--space-5);
}
</style>
