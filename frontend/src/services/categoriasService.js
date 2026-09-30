import api from './api'

export async function listarCategorias() {
  const resposta = await api.get('/api/categorias')
  return resposta.data
}
