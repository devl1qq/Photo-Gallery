import React from 'react';
import { uploadPicture } from '../../services/galleryApi';

const UploadButton = ({ authToken, albumName }) => {
  const handleUpload = async (e) => {
    e.preventDefault();

    const pictureFile = e.target.elements.pictureFile.files[0];

    if (!pictureFile) {
      alert('Please select a picture to upload.');
      return;
    }

    const pictureData = new FormData();
    pictureData.append('AlbumName', albumName);
    pictureData.append('PictureFile', pictureFile);

    try {
      await uploadPicture(pictureData, authToken); // Pass authToken to uploadPicture
      alert('Picture uploaded successfully.');
    } catch (error) {
      console.error('Error while uploading picture:', error);
      alert('Failed to upload picture. Please try again later.');
    }
  };

  return (
    <form onSubmit={handleUpload}>
      <input type="file" name="pictureFile" accept="image/*" />
      <button type="submit">Upload Photo</button>
    </form>
  );
};

export default UploadButton;
