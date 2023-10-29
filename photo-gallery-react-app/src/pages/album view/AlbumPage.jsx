import React, { Component } from 'react';
import NavBar from '../../components/navbar/NavBar';
import AlbumPhotos from '../../components/photos/AlbumPhotos';

 

class AlbumPage extends Component {
  render() {
    return (
      <div className="album-page">
        <NavBar /> 
        <div className="content">
          <AlbumPhotos />
        </div>
      </div>
    );
  }
}

export default AlbumPage;
