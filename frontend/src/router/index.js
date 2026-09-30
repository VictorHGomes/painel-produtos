import { createRouter, createWebHistory } from 'vue-router'
import ListaProdutosView from '../views/ListaProdutosView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/produtos',
    },
    {
      path: '/produtos',
      name: 'produtos',
      component: ListaProdutosView,
    },
  ],
})

export default router
