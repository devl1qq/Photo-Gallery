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

export const createAlbumInsideUserFolder = async (albumData) => {
  try {
    const response = await api.post('/gallery/create-album-inside-user-folder', albumData, getConfig());
    console.log('Album created successfully:', response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const uploadPicture = async (pictureData) => {
  try {
    const response = await api.post('/gallery/upload-picture', pictureData, getConfig());
    console.log('Picture uploaded successfully:', response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deletePicture = async (photoId) => {
  try {
    const response = await api.delete(`/gallery/delete-picture/${photoId}`, getConfig());
    console.log('Picture deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deleteAlbum = async (albumData) => {
  try {
    const response = await api.delete('/gallery/delete-album', { data: albumData, ...getConfig() });
    console.log('Album and associated pictures deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getAllMyAlbums = async () => {
  try {
    const response = await api.get('/gallery/get-all-my-albums', getConfig());
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getPicturesFromAlbum = async (albumData) => {
  try {
    const response = await api.get('/gallery/get-pictures-from-album', { params: albumData, ...getConfig() });
    return response.data;
  } catch (error) {
    throw error;
  }
};

export default {
  createAlbumInsideUserFolder,
  uploadPicture,
  deletePicture,
  deleteAlbum,
  getAllMyAlbums,
  getPicturesFromAlbum,
};
