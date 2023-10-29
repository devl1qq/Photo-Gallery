import React, { useEffect, useState } from 'react';
import './style.css';
import folderImage from './folder-svgrepo-com.svg';
import { Link } from 'react-router-dom';
import { getPhotosFromOtherUsersAlbum } from '../../services/interactApi';
import { deleteAlbumByName } from '../../services/adminApi';
import { deleteAlbum } from '../../services/galleryApi';

const AlbumCard = ({ album, onDelete }) => {
  const [coverImage, setCoverImage] = useState(null);

  useEffect(() => {
    getPhotosFromOtherUsersAlbum(album.id, 1, 1)
      .then((photos) => {
        if (photos.length > 0) {
          setCoverImage(photos[0].path);
        }
      })
      .catch((error) => {
        console.error('Failed to fetch photos:', error);
      });
  }, [album.id]);

  const handleDelete = async () => {
    try {
      // Attempt to delete the album by name
      await deleteAlbumByName(album.name);
      console.log('Album deleted by name successfully');
      // Notify the parent component (AlbumsGrid) to update the albums array
      onDelete(album.id, album.name); // Pass both albumId and albumName
    } catch (error) {
      console.error('Failed to delete album by name:', error);
      try {
        // If deleting by name fails, attempt to delete by ID
        await deleteAlbum({ albumName: album.name });
        console.log('Album deleted by ID successfully');
        // Notify the parent component (AlbumsGrid) to update the albums array
        onDelete(album.id, album.name); // Pass both albumId and albumName
      } catch (error) {
        console.error('Failed to delete album by ID:', error);
      }
    }
  };

  return (
    <div className="album-card">
      <Link to={`/album/${album.id}/${album.name}`}> {/* Pass both albumId and albumName */}
        <h3>{album.name}</h3>
        {coverImage ? (
          <img src={coverImage} alt={album.name} />
        ) : (
          <img src={folderImage} alt={album.name} />
        )}
      </Link>
      <button onClick={handleDelete}>Delete</button>
    </div>
  );
};

export default AlbumCard;
