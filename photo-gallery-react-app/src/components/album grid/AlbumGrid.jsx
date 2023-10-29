import React, { useState, useEffect } from 'react';
import AlbumCard from './AlbumCard';
import { getOtherUsersAlbums } from '../../services/interactApi';
import { getAllMyAlbums, createAlbumInsideUserFolder } from '../../services/galleryApi';
import './style.css';

function AlbumsGrid() {
  const [albums, setAlbums] = useState([]);
  const [authToken, setAuthToken] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const albumsPerPage = 5;
  const [showMyAlbums, setShowMyAlbums] = useState(false);
  const [newAlbumName, setNewAlbumName] = useState('');

  useEffect(() => {
    checkAuthToken();
    if (showMyAlbums) {
      fetchMyAlbums(currentPage);
    } else {
      fetchOtherUsersAlbums(currentPage);
    }
  }, [showMyAlbums, currentPage]);

  const checkAuthToken = () => {
    const storedAuthToken = localStorage.getItem('authToken');
    if (storedAuthToken) {
      setAuthToken(storedAuthToken);
    }
  };

  const fetchOtherUsersAlbums = async (page) => {
    try {
      const response = await getOtherUsersAlbums(page, albumsPerPage);
      setAlbums(response);
    } catch (error) {
      console.error('Failed to fetch albums of others:', error);
    }
  };

  const fetchMyAlbums = async (page) => {
    try {
      const myAlbums = await getAllMyAlbums(page, albumsPerPage);
      setAlbums(myAlbums);
    } catch (error) {
      console.error('Failed to fetch my albums:', error);
    }
  };

  const handleCreateNewAlbum = async () => {
    try {
      const newAlbumData = {
        albumName: newAlbumName,
        // Add other necessary data for the new album here
      };
      await createAlbumInsideUserFolder(newAlbumData);
      console.log('New album created successfully');
      // After creating a new album, you may want to update the album list:
      if (showMyAlbums) {
        fetchMyAlbums(currentPage);
      } else {
        fetchOtherUsersAlbums(currentPage);
      }
      setNewAlbumName(''); // Clear the input field
    } catch (error) {
      console.error('Failed to create a new album:', error);
    }
  };

  const toggleShowMyAlbums = () => {
    setShowMyAlbums(!showMyAlbums);
    setCurrentPage(1); // Reset page when switching between My Albums and Albums of Others
  };

  const handlePageChange = (newPage) => {
    setCurrentPage(newPage);
  };

  // Define the handleDeleteAlbum function
  const handleDeleteAlbum = (albumId) => {
    // This function should update the album list by removing the deleted album.
    // You can filter the albums array to exclude the deleted album with the given albumId.
    setAlbums((prevAlbums) => prevAlbums.filter((album) => album.id !== albumId));
  };

  return (
    <div className="albums-grid">
      <div className="grid-buttons">
        {authToken && (
          <div>
            <button onClick={toggleShowMyAlbums}>
              {showMyAlbums ? 'Albums of Others' : 'My Albums'}
            </button>
            {showMyAlbums && (
              <div>
                <input
                  type="text"
                  placeholder="New Album Name"
                  value={newAlbumName}
                  onChange={(e) => setNewAlbumName(e.target.value)}
                />
                <button onClick={handleCreateNewAlbum}>Create New Album</button>
              </div>
            )}
          </div>
        )}
      </div>
      <div className="album-cards">
        {albums.map((album) => (
          <AlbumCard key={album.id} album={album} onDelete={handleDeleteAlbum} />
        ))}
      </div>
      <div className="pagination">
        <button onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>
          Previous
        </button>
        <button onClick={() => handlePageChange(currentPage + 1)}>Next</button>
      </div>
    </div>
  );
}

export default AlbumsGrid;
