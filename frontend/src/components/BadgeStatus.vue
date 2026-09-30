<script setup>
defineProps({
  ativo: { type: Boolean, required: true },
})
</script>

<template>
  <span class="status" :class="ativo ? 'status-ativo' : 'status-inativo'">
    <span class="status-marca" aria-hidden="true"></span>
    {{ ativo ? 'Ativo' : 'Inativo' }}
  </span>
</template>

<style scoped>
.status {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  font-weight: 500;
}

.status-marca {
  width: 8px;
  height: 8px;
  flex-shrink: 0;
}

.status-ativo {
  color: var(--color-success);
}

.status-ativo .status-marca {
  background: var(--color-success);
}

.status-inativo {
  color: var(--color-text-secondary);
}

.status-inativo .status-marca {
  background: var(--color-text-secondary);
}
</style>
