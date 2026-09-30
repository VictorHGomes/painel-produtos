<script setup>
defineProps({
  estoque: { type: Number, required: true },
})
</script>

<template>
  <span v-if="estoque === 0" class="badge badge-perigo">Sem estoque</span>
  <span v-else class="numerico" :class="{ 'texto-alerta': estoque < 10 }">{{ estoque }}</span>
</template>
