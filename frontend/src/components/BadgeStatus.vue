<script setup>
defineProps({
  ativo: { type: Boolean, required: true },
})
</script>

<template>
  <span class="status" :class="ativo ? 'status-ativo' : 'status-inativo'">
    {{ ativo ? 'Ativo' : 'Inativo' }}
  </span>
</template>

<style scoped>
.status {
  font-weight: 500;
}

.status-ativo {
  color: var(--color-success);
}

.status-inativo {
  color: var(--color-text-secondary);
}
</style>
