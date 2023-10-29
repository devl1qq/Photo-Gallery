import React, { useState } from 'react';
import { uploadPicture } from '../../services/galleryApi';

const UploadButton = ({ albumName }) => {
  const [selectedFile, setSelectedFile] = useState(null);

  const handleFileChange = (e) => {
    setSelectedFile(e.target.files[0]);
  };

  const handleUpload = async (e) => {
    e.preventDefault();

    if (!selectedFile) {
      alert('Please select a picture to upload.');
      return;
    }

    const authToken = localStorage.getItem('authToken');

    if (!authToken) {
      alert('Authentication token is missing.');
      return;
    }

    try {
      const pictureData = new FormData();
      pictureData.append('AlbumName', albumName);
      pictureData.append('PictureFile', selectedFile);

      await uploadPicture(albumName, pictureData, authToken);
      alert('Picture uploaded successfully.');
    } catch (error) {
      console.error('Error while uploading picture:', error);
      alert('Failed to upload picture. Please try again later.');
    }
  };

  return (
    <form onSubmit={handleUpload}>
      <input
        type="file"
        name="pictureFile"
        accept="image/*"
        onChange={handleFileChange}
      />
      <button type="submit">Upload Photo</button>
    </form>
  );
};

export default UploadButton;
