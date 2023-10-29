import axios from 'axios';

const baseUrl = process.env.REACT_APP_API_BASE_URL;

const api = axios.create({
  baseURL: baseUrl,
});

const getConfig = () => {
  const authToken = localStorage.getItem('authToken');
  if (authToken) {
    return {
      headers: {
        Authorization: `Bearer ${authToken}`,
      },
    };
  }
  return {};
};

export const getAllUsers = async () => {
  try {
    const response = await api.get('/admin/users', getConfig());
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getAlbumsByUserId = async (userId) => {
  try {
    const response = await api.get(`/admin/albums/${userId}`, getConfig());
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getPicturesByAlbumId = async (albumId) => {
  try {
    const response = await api.get(`/admin/pictures/${albumId}`, getConfig());
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deleteAlbumByName = async (albumName) => {
  try {
    const response = await api.delete(`/admin/album/${albumName}`, getConfig());
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deletePictureById = async (pictureId) => {
  try {
    const response = await api.delete(`/admin/picture/${pictureId}`, getConfig());
    return response.data;
  } catch (error) {
    throw error;
  }
};

export default {
  getAllUsers,
  getAlbumsByUserId,
  getPicturesByAlbumId,
  deleteAlbumByName,
  deletePictureById,
};
