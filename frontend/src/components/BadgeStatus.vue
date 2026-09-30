<script setup>
defineProps({
  ativo: { type: Boolean, required: true },
})
</script>

<template>
  <span class="badge" :class="ativo ? 'badge-sucesso' : 'badge-neutro'">
    {{ ativo ? 'Ativo' : 'Inativo' }}
  </span>
</template>
