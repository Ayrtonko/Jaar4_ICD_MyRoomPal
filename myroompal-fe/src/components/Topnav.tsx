import {Navbar, Nav, Container} from "react-bootstrap";
import {Link} from "react-router-dom";
import {useAuth0} from "@auth0/auth0-react";
import Brand from '../Assets/TextMyRoomPal_1.svg'
import '../css/home.css'
import {useUserRole} from 'helpers/useUserRole';

const routes = [
  { path: '/', label: 'Home', authRequired: false },
  { path: '/Match', label: 'Match', authRequired: true },
  { path: '/Rooms', label: 'Rooms', authRequired: false },
  { path: '/Listing', label: 'Listing', authRequired: true },
  { path: '/Report', label: 'Report', authRequired: true },
  { path: '/profile', label: 'Profile', authRequired: true },
];

const Topnav = () => {
  const { isAuthenticated, logout, loginWithRedirect } = useAuth0();
  const userRole = useUserRole();
  const isAdmin = userRole === 'Moderator';



  return (
    <Navbar collapseOnSelect expand="lg" className="navbar navbar-dark bg-dark">
      <Container>
        <Navbar.Brand as={Link} to="/">
          <img className= "brand" src={Brand} alt="Logo MyRoomPal"
            style={{
              width: "150px",
              height: "30px",
            }} />
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto">
            {routes.map((route) => {
              if (route.authRequired && !isAuthenticated) {
                return null;
              }
              return (
                <Nav.Link as={Link} to={route.path} key={route.path} cy-data={route.label}>
                  {route.label}
                </Nav.Link>
              );
            })}
            {isAdmin && (
              <Nav.Link as={Link} to="/admin">
                Admin
              </Nav.Link>
            )}
          </Nav>
          <Nav className="ms-auto">
            {isAuthenticated ? (
              <button
                className="btn btn-outline-primary"
                onClick={() =>
                  logout({ logoutParams: { returnTo: window.location.origin } })
                }
              >
                Log Out
              </button>
            ) : (
              <button
                className="btn btn-outline-primary"
                onClick={() => loginWithRedirect()}
                cy-data="logIn-btn"
              >
                Log In
              </button>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default Topnav;
