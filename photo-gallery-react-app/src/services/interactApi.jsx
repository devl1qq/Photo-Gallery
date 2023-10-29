import axios from 'axios';

const baseUrl = process.env.REACT_APP_API_BASE_URL;

const api = axios.create({
  baseURL: baseUrl,
});

export const likeOrDislikePhoto = async (photoId, like, authToken) => {
  try {
    const response = await api.post(`/interaction/like-dislike?like=${like}`, `"${photoId}"`, {
      headers: {
        Authorization: `Bearer ${authToken}`,
        'Content-Type': 'application/json',
        'accept': 'text/plain',
      },
    });
    return response.data;
  } catch (error) {
    throw error;
  }
};


  export const getOtherUsersAlbums = async (page, pageSize) => {
    try {
      console.log('Sending request to /interaction/other-users-albums with params:', { page, pageSize });
      const response = await api.get('/interaction/other-users-albums', {
        params: { page, pageSize },
      });
      console.log('Response received:', response);
      return response.data;
    } catch (error) {
      console.error('Error while making the request:', error);
      throw error;
    }
  };
  


  export const getPhotosFromOtherUsersAlbum = async (albumId, page, pageSize) => {
    try {
      console.log('Calling getPhotosFromOtherUsersAlbum with albumId:', albumId, 'page:', page, 'pageSize:', pageSize);
      const url = `/interaction/other-users-album-photos?albumId=${albumId}&page=${page}&pageSize=${pageSize}`;
      const response = await api.get(url);
  
      console.log('API response:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error in getPhotosFromOtherUsersAlbum:', error);
      throw error;
    }
  };
  
  

  export const findInteraction = async (photoId, authToken) => {
    try {
      const response = await api.get('/interaction/find-interaction', {
        params: { photoId },
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      });
  
      return response.data;
    } catch (error) {
      throw error;
    }
  };
  

export default {
    likeOrDislikePhoto,
    getOtherUsersAlbums,
    getPhotosFromOtherUsersAlbum,
    findInteraction
  };
  
