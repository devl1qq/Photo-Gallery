import axios from 'axios';

const baseUrl = process.env.REACT_APP_API_BASE_URL;

const api = axios.create({
  baseURL: baseUrl,
});

const signup = async (signupData) => {
  try {
    const response = await api.post('/auth/signup', signupData);
    console.log('Signup successful:', response.data); // Log the response
    return response.data;
  } catch (error) {
    throw error;
  }
};

const signin = async (signinData) => {
  try {
    const response = await api.post('/auth/signin', signinData);
    const authToken = response.data.token; 
    console.log('Signin successful:', authToken);

    localStorage.setItem('authToken', authToken);

    return authToken;
  } catch (error) {
    throw error;
  }
};

export default {
  signup,
  signin,
};
