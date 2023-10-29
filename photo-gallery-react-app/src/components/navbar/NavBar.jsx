import React, { Component } from 'react';
import './style.css';
import authService from '../../services/authApi';
import { Link } from 'react-router-dom';

class NavBar extends Component {
  constructor(props) {
    super(props);
    this.state = {
      showSignUp: false,
      showSignIn: false,
      showBackButton: false,
      username: '',
      password: '',
      authToken: localStorage.getItem('authToken'), 
      albums: [],
    };
  }

  toggleSignUp = () => {
    this.setState({
      showSignUp: true,
      showSignIn: false,
      showBackButton: true,
    });
  };

  toggleSignIn = () => {
    this.setState({
      showSignUp: false,
      showSignIn: true,
      showBackButton: true,
    });
  };

  handleBack = () => {
    this.setState({
      showSignUp: false,
      showSignIn: false,
      showBackButton: false,
      username: '',
      password: '',
    });
  };

  handleSignUp = async () => {
    try {
      const signupData = {
        username: this.state.username,
        password: this.state.password,
        roleType: "User",
      };

      const authToken = await authService.signup(signupData);
      this.setState({ authToken });

      this.handleSignIn();
    } catch (error) {
      console.error('Signup failed:', error);
    }
  };

  handleSignIn = async () => {
    try {
      const signinData = {
        username: this.state.username,
        password: this.state.password,
      };

      const authToken = await authService.signin(signinData);
      this.setState({ authToken });

      // Save the authToken in localStorage
      localStorage.setItem('authToken', authToken);

      const albums = await this.fetchUserAlbums();
      this.setState({ albums });
    } catch (error) {
      console.error('Signin failed:', error);
    }
  };

  render() {
    return (
      <div className="navbar">
         <Link to="/" className="title">Photo Gallery</Link>
        <div>
          {!this.state.showBackButton && !this.state.authToken && (
            <div>
              <button onClick={this.toggleSignUp}>Sign Up</button>
              <button onClick={this.toggleSignIn}>Sign In</button>
            </div>
          )}
          {this.state.showSignUp && !this.state.authToken && (
            <div>
              <input
                type="text"
                placeholder="Enter username"
                value={this.state.username}
                onChange={(e) => this.setState({ username: e.target.value })}
              />
              <input
                type="password"
                placeholder="Enter password"
                value={this.state.password}
                onChange={(e) => this.setState({ password: e.target.value })}
              />
              <button onClick={this.handleSignUp}>Sign Up</button>
              {this.state.showBackButton && (
                <button onClick={this.handleBack}>Back</button>
              )}
            </div>
          )}
          {this.state.showSignIn && !this.state.authToken && (
            <div>
              <input
                type="text"
                placeholder="Enter username"
                value={this.state.username}
                onChange={(e) => this.setState({ username: e.target.value })}
              />
              <input
                type="password"
                placeholder="Enter password"
                value={this.state.password}
                onChange={(e) => this.setState({ password: e.target.value })}
              />
              <button onClick={this.handleSignIn}>Sign In</button>
              {this.state.showBackButton && (
                <button onClick={this.handleBack}>Back</button>
              )}
            </div>
          )}
          {this.state.authToken && (
            <div>
              <p>Welcome {this.state.username}!</p>
            </div>
          )}
        </div>
      </div>
    );
  }
}

export default NavBar;
