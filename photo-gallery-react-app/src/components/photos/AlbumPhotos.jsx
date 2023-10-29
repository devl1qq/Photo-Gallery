import React, { useEffect, useState } from 'react';
import './style.css';
import { useParams } from 'react-router-dom';
import { getPhotosFromOtherUsersAlbum } from '../../services/interactApi';
import PhotoCard from './PhotoCard';
import UploadButton from './UploadButton';

const AlbumPhotos = ({ authToken }) => {
  const { albumId, albumName } = useParams(); 
  const [photos, setPhotos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchAlbumPhotos() {
      try {
        const fetchedPhotos = await getPhotosFromOtherUsersAlbum(albumId, 1, 5); 
        setPhotos(fetchedPhotos);
        setLoading(false);
      } catch (error) {
        setError(error);
        setLoading(false);
      }
    }

    fetchAlbumPhotos();
  }, [albumId]);

  const handleDelete = (deletedPhotoId) => {
    setPhotos((prevPhotos) => prevPhotos.filter((photo) => photo.id !== deletedPhotoId));
  };

  const handleUpload = (newPhoto) => {
    setPhotos((prevPhotos) => [newPhoto, ...prevPhotos]);
  };

  return (
    <div className="album-photos">
      <UploadButton authToken={authToken} albumName={albumName} onUpload={handleUpload} />
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>Error: {error.message}</p>
      ) : (
        <div>
          {photos.length > 0 ? (
            <div className="photo-list">
              {photos.map((photo) => (
                <PhotoCard key={photo.id} photo={photo} authToken={authToken} onDelete={handleDelete} />
              ))}
            </div>
          ) : (
            <div className="empty-album">
              <h2>No photos in this album</h2>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default AlbumPhotos;
