import React from "react";
import { useAuth0 } from "@auth0/auth0-react"; // Auth0 hook
import "./Footer.css";

const Footer = () => {
  const { isAuthenticated } = useAuth0(); // Controleer of de gebruiker is ingelogd

  return (
    <footer style={{ backgroundColor: "#093A55", color: "#eee", marginTop: "40px", padding: "40px 0" }}>
      <div
        style={{
          maxWidth: "1200px",
          margin: "0 auto",
          display: "flex",
          justifyContent: "space-between",
          flexWrap: "wrap",
          padding: "0 20px",
          gap: "20px",
        }}
      >
        {/* MyRoomPal Title */}
        <div style={{ flex: "1 1 300px", minWidth: "200px" }}>
          <h2 className="footer-title" style={{ color: "white", fontWeight: "bold" }}>
            MyRoomPal
          </h2>
        </div>

        {/* Navigation */}
<div style={{ flex: "1 1 300px", textAlign: "left", minWidth: "200px" }}>
  <h4 className="footer-title" style={{ color: "#fff", fontWeight: "bold", marginBottom: "10px" }}>
    Navigation
  </h4>

  {!isAuthenticated ? (
    // Navigatie voor niet-ingelogde gebruikers
    <ul
      style={{
        listStyleType: "none",
        padding: 0,
        margin: 0,
        display: "flex",
        flexDirection: "column",
        gap: "5px",
      }}
    >
      <li>
        <a href="/" className="footer-link">Home</a>
      </li>
      <li>
        <a href="/Rooms" className="footer-link">Rooms</a>
      </li>
    </ul>
  ) : (
    // Navigatie voor ingelogde gebruikers
      <ul
          style={{
              listStyleType: "none",
              padding: 0,
              margin: 0,
              display: "flex",
              flexWrap: "wrap", // Twee kolommen
              columnGap: "0px",
              rowGap: "5px",
          }}
      >
          <li style={{width: "40%"}}>
              <a href="/" className="footer-link">Home</a>
          </li>
          <li style={{width: "40%"}}>
              <a href="/Rooms" className="footer-link">Rooms</a>
          </li>
          <li style={{width: "40%"}}>
              <a href="/Match" className="footer-link">Match</a>
          </li>
          <li style={{width: "40%"}}>
              <a href="/Report" className="footer-link">Report</a>
          </li>
          <li style={{width: "40%"}}>
              <a href="/profile" className="footer-link">Profile</a>
          </li>
          <li style={{width: "45%"}}>
              <a href="/profileEditPage" className="footer-link">Edit Profile</a>
          </li>
      </ul>
  )}
</div>


        {/* Contact */}
        <div style={{ flex: "1 1 300px", textAlign: "left", minWidth: "200px" }}>
          <h4 className="footer-title" style={{ color: "#fff", fontWeight: "bold", marginBottom: "10px" }}>
            Contact
          </h4>
          <ul style={{ whiteSpace: "nowrap", listStyleType: "none", padding: 0, margin: 0, lineHeight: "1.8" }}>
            <li>Email: support@myroompal.com</li>
            <li>Phone: +31 6 123 456 78</li>
            <li>Address: Amsterdam, Netherlands</li>
            <li>Open: Mon-Fri, 9 AM - 6 PM</li>
          </ul>
        </div>

        {/* Follow Us */}
        <div style={{ flex: "1 1 200px", textAlign: "left", minWidth: "200px" }}>
          <h4 className="footer-title" style={{ color: "#fff", fontWeight: "bold", marginBottom: "10px" }}>
            Follow Us
          </h4>
          <ul
            style={{
              listStyleType: "none",
              padding: 0,
              margin: 0,
              lineHeight: "1.8",
            }}
          >
            <li><a href="https://facebook.com" className="footer-link">Facebook</a></li>
            <li><a href="https://twitter.com" className="footer-link">Twitter</a></li>
            <li><a href="https://instagram.com" className="footer-link">Instagram</a></li>
          </ul>
        </div>
      </div>

      {/* Line and Copyright */}
      <div style={{ maxWidth: "1200px", margin: "20px auto", padding: "0 20px" }}>
        <hr style={{ borderColor: "#fff", borderWidth: "1px", margin: "20px 0" }} />
        <p style={{ textAlign: "center", fontSize: "14px", margin: 0 }}>
          © 2024 MyRoomPal. All rights reserved.
        </p>
      </div>
    </footer>
  );
};

export default Footer;













