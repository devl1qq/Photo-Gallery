import React, { useState, useEffect } from 'react';
import './style.css';
import { likeOrDislikePhoto, findInteraction } from '../../services/interactApi';
import { deletePictureById } from '../../services/adminApi';
import { deletePicture } from '../../services/galleryApi'

const PhotoCard = ({ photo }) => {
  const [likes, setLikes] = useState(photo.likes);
  const [dislikes, setDislikes] = useState(photo.dislikes);
  const [isLiked, setIsLiked] = useState(false);
  const [isDisliked, setIsDisliked] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Retrieve the authToken from local storage
    const authToken = localStorage.getItem('authToken');

    findInteraction(photo.id, authToken)
      .then((interaction) => {
        if (interaction) {
          setIsLiked(interaction.isLiked);
          setIsDisliked(interaction.isDisliked);
        }
      })
      .catch((error) => {
        setError('Error while checking photo interaction. Please try again later.');
      });
  }, [photo.id]);

  const handleLikeClick = async () => {
    // Retrieve the authToken from local storage
    const authToken = localStorage.getItem('authToken');

    if (!isLiked) {
      try {
        await likeOrDislikePhoto(photo.id, true, authToken);
        setLikes(likes + 1);
        setIsLiked(true);

        if (isDisliked) {
          setDislikes(dislikes - 1);
          setIsDisliked(false);
        }
      } catch (error) {
        setError('Error while liking the photo. Please try again later.');
      }
    }
  };

  const handleDislikeClick = async () => {
    // Retrieve the authToken from local storage
    const authToken = localStorage.getItem('authToken');

    if (!isDisliked) {
      try {
        await likeOrDislikePhoto(photo.id, false, authToken);
        setDislikes(dislikes + 1);
        setIsDisliked(true);

        if (isLiked) {
          setLikes(likes - 1);
          setIsLiked(false);
        }
      } catch (error) {
        setError('Error while disliking the photo. Please try again later.');
      }
    }
  };

  const handleDeleteClick = async () => {
    try {
      // Attempt to delete the photo by name
      await deletePictureById(photo.id);
      console.log('Photo deleted by name successfully');
      // Handle successful deletion (e.g., remove the card from the UI).
    } catch (error) {
      console.error('Failed to delete photo by name:', error);
      try {
        // If deleting by name fails, attempt to delete by ID
        await deletePicture(photo.id);
        console.log('Photo deleted by ID successfully');
        // Handle successful deletion (e.g., remove the card from the UI).
      } catch (error) {
        setError('Error while deleting the photo. Please try again later.');
      }
    }
  };
  

  return (
    <div className="photo-card">
      <img src={photo.path} alt={photo.id} />
      <div className="likes-dislikes">
        <button
          className={`like-button ${isLiked ? 'liked' : ''}`}
          onClick={handleLikeClick}
        >
          Like {likes}
        </button>
        <button
          className={`dislike-button ${isDisliked ? 'disliked' : ''}`}
          onClick={handleDislikeClick}
        >
          Dislike {dislikes}
        </button>
        <button className="delete-button" onClick={handleDeleteClick}>
          Delete
        </button>
      </div>
      {error && <p className="error-message">{error}</p>}
    </div>
  );
};

export default PhotoCard;
