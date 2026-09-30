<script setup>
import { computed } from 'vue'

const props = defineProps({
  estoque: { type: Number, required: true },
})

const titulo = computed(() => {
  if (props.estoque === 0) return 'Sem estoque'
  if (props.estoque < 10) return 'Estoque baixo'
  return undefined
})
</script>

<template>
  <span
    class="numerico"
    :class="{ 'estoque-zerado': estoque === 0, 'estoque-baixo': estoque > 0 && estoque < 10 }"
    :title="titulo"
  >
    {{ estoque }}
  </span>
</template>

<style scoped>
.estoque-zerado {
  color: var(--color-danger);
  font-weight: 700;
}

.estoque-baixo {
  color: var(--color-warning);
  font-weight: 600;
}
</style>
