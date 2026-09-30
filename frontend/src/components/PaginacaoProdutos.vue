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
    <button class="botao" :disabled="pagina <= 1" @click="$emit('anterior')">Anterior</button>
    <span>Página {{ pagina }} de {{ totalPaginas || 1 }} ({{ totalItens }} produtos)</span>
    <button class="botao" :disabled="pagina >= totalPaginas" @click="$emit('proxima')">
      Próxima
    </button>
  </div>
</template>

<style scoped>
.paginacao {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  margin-top: 1.5rem;
  flex-wrap: wrap;
}
</style>
