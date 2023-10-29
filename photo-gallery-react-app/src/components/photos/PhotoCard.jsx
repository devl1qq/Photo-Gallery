import React, { useState, useEffect } from 'react';
import './style.css';
import { likeOrDislikePhoto, findInteraction } from '../../services/interactApi';

const PhotoCard = ({ photo, authToken }) => {
  const [likes, setLikes] = useState(photo.likes);
  const [dislikes, setDislikes] = useState(photo.dislikes);
  const [isLiked, setIsLiked] = useState(false);
  const [isDisliked, setIsDisliked] = useState(false);

  useEffect(() => {
    // Check if the user has already liked or disliked this photo
    findInteraction(photo.id, authToken)
      .then((interaction) => {
        if (interaction) {
          setIsLiked(interaction.isLiked);
          setIsDisliked(interaction.isDisliked);
        }
      })
      .catch((error) => {
        console.error('Error while checking photo interaction:', error);
      });
  }, [photo.id, authToken]);

  const handleLikeClick = async () => {
    if (!isLiked) {
      try {
        await likeOrDislikePhoto(photo.id, true, authToken);
        setLikes(likes + 1);
        setIsLiked(true);

        // If the user previously disliked the photo, update the state
        if (isDisliked) {
          setDislikes(dislikes - 1);
          setIsDisliked(false);
        }
      } catch (error) {
        console.error('Error while liking the photo:', error);
      }
    }
  };

  const handleDislikeClick = async () => {
    if (!isDisliked) {
      try {
        await likeOrDislikePhoto(photo.id, false, authToken);
        setDislikes(dislikes + 1);
        setIsDisliked(true);

        // If the user previously liked the photo, update the state
        if (isLiked) {
          setLikes(likes - 1);
          setIsLiked(false);
        }
      } catch (error) {
        console.error('Error while disliking the photo:', error);
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
      </div>
    </div>
  );
};

export default PhotoCard;
