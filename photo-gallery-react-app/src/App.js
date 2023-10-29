import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import MainPage from './pages/main page/MainPage';
import AlbumPage from './pages/album view/AlbumPage';

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/album/:albumId" element={<AlbumPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
