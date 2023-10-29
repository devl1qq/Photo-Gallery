import React, { Component } from 'react';
import NavBar from '../../components/navbar/NavBar'; 
import AlbumsGrid from '../../components/album grid/AlbumGrid'; 

class MainPage extends Component {
  render() {
    return (
      <div className="main-page">
        <NavBar />        
        <div className="content">
          <AlbumsGrid />
        </div>
      </div>
    );
  }
}

export default MainPage;
