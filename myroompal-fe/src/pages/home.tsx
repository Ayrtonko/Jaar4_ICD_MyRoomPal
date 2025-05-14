import { Link, useNavigate } from 'react-router-dom';
import Logo from '../Assets/LogoCorrect.svg'
import Search from '../Assets/search.png'
import Agreement from '../Assets/handshake.png'
import House from '../Assets/house.png'
import os1 from '../Assets/Cantoria, Almería.png';
import os2 from '../Assets/Catral, Alicante.png';
import os3 from '../Assets/Chercos, Almería.png';
import '../Assets/Madrid.jpg'
import '../css/home.css';
import {Carousel} from 'react-bootstrap';
import { useAuth0 } from '@auth0/auth0-react';


function Home() {
  const {isAuthenticated, loginWithRedirect} = useAuth0();
  const navigate = useNavigate();

  const handleMatchLogin = async () => {
    if(!isAuthenticated) {
      await loginWithRedirect({
        appState: {returnTo: '/match'}
      });
    } else {
      navigate('/match');
    }
  };

    return (
      <div>
      <div className="homecontainer">
        <div className='madrid-city-banner'>
      <div className="Hometext">
      <div className='title'>MyRoomPal</div>
      <br/>
      <button className='rounded-3' onClick={handleMatchLogin}>Find a match</button>
      </div>
      </div>
      </div>

      <h1 className='h1caurosel'>Newly available rooms</h1>
      <div className='Caurosel'>
      <Carousel>
        <Carousel.Item interval={4500}>
          <img
            className="d-block w-100"src={os1}
            alt="Cantoria, Almería"/>
          <Carousel.Caption>
          <div className='item'>Cantoria, Almería</div>
          </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item interval={4500}>
          <img
            className="d-block w-100"
        src={os2}
            alt="Catral, Alicante"
          />
          <Carousel.Caption>
          <div className='item'>Catral, Alicante</div>
          </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item interval={4500}>
          <img
            className="d-block w-100"
        src={os3}
            alt="Chercos, Almería" />
          <Carousel.Caption>
          <div className='item'>Chercos, Almería</div>
          </Carousel.Caption>
        </Carousel.Item>
      </Carousel>
    </div>

    <div className='Info'>
  <h1>Renting Process explained</h1>
  <div className='Cards'>
    <div className='Card'>
      <h1>Find</h1><br />
      <img src={Search} alt="Magnifer" className='Search' /><br />
      <p>At MyRoomPal, we don't just find your ideal home, but also the perfect roommates. Our smart matching system compares your preferences, such as location, budget, and lifestyle, to find the best match. This way, you can truly make your new house feel like home!</p>
    </div>
    <div className='Card'>
      <h1>Agree</h1><br />
      <img src={Agreement} alt="Handshake" className='Agreement'/><br />
      <p>At MyRoomPal, we make sure all agreements between you, your roommates, and the landlord are clear and transparent. With fair terms and open communication, you can start your rental journey with confidence</p>
    </div>
    <div className='Card'>
      <h1>Home!</h1><br />
      <img src={House} alt="House" className='House'/><br />
      <p>"Congratulations! You've found your new home with MyRoomPal. Settle in, connect with your roommates, and enjoy the comfort and convenience of a space that truly fits your lifestyle."</p>
    </div>
  </div>
</div>
      <div className='MyRoomPalInfo'>
      <div className='about'>
        <h1>About MyRoomPal</h1> <br />
        <img className= "logo" src={Logo} alt="Logo MyRoomPal" />
        <p></p>
        <p>"At MyRoomPal, we make finding the perfect home and roommates simple and stress-free. Our platform uses advanced matching technology to connect you with homes and housemates that suit your budget, location, and lifestyle preferences.

We believe that a house is more than just four walls – it’s about the people and experiences that make it feel like home. Whether you’re looking for a cozy room, reliable housemates, or a smooth rental process, MyRoomPal is here to help you every step of the way.

Find your place. Meet your people. Start living your best life with MyRoomPal."</p>
      </div>
      </div>
      </div>
    );
  }
  
  export default Home;