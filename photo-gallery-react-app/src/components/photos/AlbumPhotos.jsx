import React, { useState, useEffect } from 'react';
import './style.css';
import folderImage from '../album grid/folder-svgrepo-com.svg';
import { useParams } from 'react-router-dom';
import { getPhotosFromOtherUsersAlbum } from '../../services/interactApi';
import PhotoCard from './PhotoCard';

const AlbumPhotos = ({ authToken }) => {
  const { albumId } = useParams();
  const [album, setAlbum] = useState(null);
  const [photos, setPhotos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchAlbumPhotos() {
      try {
        const { album, photos } = await getPhotosFromOtherUsersAlbum(albumId, 1, 10);
        setAlbum(album);
        setPhotos(photos);
        setLoading(false);
      } catch (error) {
        setError(error);
        setLoading(false);
      }
    }

    fetchAlbumPhotos();
  }, [albumId]);

  return (
    <div>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>Error: {error.message}</p>
      ) : album ? (
        <div>
          <h2>Album Name: {album.name}</h2>
          {photos.length > 0 ? (
            <div className="photo-list">
              {photos.map((photo) => (
                <div key={photo.id} className="photo-item">
                  <PhotoCard photo={photo} authToken={authToken} />
                </div>
              ))}
            </div>
          ) : (
            <div>
              <h2>No photos in this album</h2>
            </div>
          )}
        </div>
      ) : (
        <div>
          <h2>Album Not Found</h2>
          <img src={folderImage} alt="Album Not Found" />
        </div>
      )}
    </div>
  );
};

export default AlbumPhotos;
