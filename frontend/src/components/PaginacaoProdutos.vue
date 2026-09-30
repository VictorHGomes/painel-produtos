<script setup>
defineProps({
  pagina: { type: Number, required: true },
  totalPaginas: { type: Number, required: true },
  totalItens: { type: Number, required: true },
})

defineEmits(['anterior', 'proxima'])
</script>

<template>
  <div class="paginacao">
    <span class="paginacao-total">{{ totalItens }} produto(s)</span>

    <div class="paginacao-controles">
      <button class="btn btn-discreto" :disabled="pagina <= 1" @click="$emit('anterior')">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <polyline points="15 18 9 12 15 6" />
        </svg>
        Anterior
      </button>

      <span class="paginacao-texto">Página {{ pagina }} de {{ totalPaginas || 1 }}</span>

      <button class="btn btn-discreto" :disabled="pagina >= totalPaginas" @click="$emit('proxima')">
        Próxima
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <polyline points="9 18 15 12 9 6" />
        </svg>
      </button>
    </div>
  </div>
</template>

<style scoped>
.paginacao {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: var(--space-3);
  padding: var(--space-4);
  border-top: 1px solid var(--color-border);
}

.paginacao-total {
  color: var(--color-text-secondary);
  font-size: 0.85rem;
}

.paginacao-controles {
  display: flex;
  align-items: center;
  gap: var(--space-3);
  flex-wrap: wrap;
}

.paginacao-texto {
  color: var(--color-text-secondary);
  font-size: 0.85rem;
  white-space: nowrap;
}
</style>
