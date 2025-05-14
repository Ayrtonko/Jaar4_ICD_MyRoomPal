import axios from "axios";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { Row, Col, Form, Button } from "react-bootstrap";
import { useState } from "react";
import {config} from "../config";
import Logo from "../Assets/LogoCorrect.svg";
import Room from "../Assets/room.png";
import Person from "../Assets/person.png";
import NameLogo from "../Assets/TextMyRoomPal_1.svg";
import "../css/listing.css";
import "react-toastify/dist/ReactToastify.css";

function Listing() {
  const [validated, setValidated] = useState(false);
  const navigate = useNavigate();

  const [roomName, setRoomName] = useState("");
  const [description, setDescription] = useState("");
  const [rentPrice, setRentPrice] = useState("");
  const [size, setSize] = useState("");
  const [imageLink, setImages] = useState<string[]>([]);
  const [country, setCountry] = useState("");
  const [city, setCity] = useState("");
  const [streetName, setStreetName] = useState("");
  const [postalCode, setPostalCode] = useState("");

  const handleRoomNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRoomName(event.target.value);
  };

  const handleDescriptionChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setDescription(event.target.value);
  };

  const handleRentPriceChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setRentPrice(event.target.value);
  };

  const handleSizeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSize(event.target.value);
  };

  const handleCountryChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setCountry(event.target.value);
  };

  const handleCityChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setCity(event.target.value);
  };
  const handleStreetNameChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setStreetName(event.target.value);
  };
  const handlePostalCodeChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setPostalCode(event.target.value);
  };

  const handleFileChange = () => {
    setImages([
      "https://images.unsplash.com/photo-1568605114967-8130f3a36994?crop=entropy&cs=tinysrgb&fit=crop&w=400&h=300&q=80",
    ]);
  };

  interface FormEvent extends React.FormEvent<HTMLFormElement> {}

  const handleSubmit = async (event: FormEvent) => {
    const form = event.currentTarget;
    event.preventDefault();

    if (form.checkValidity() === false) {
      event.stopPropagation();
      setValidated(true);
      return;
    }

    const formData = new FormData();
    formData.append("roomName", roomName);
    formData.append("description", description);
    formData.append("rentPrice", rentPrice);
    formData.append("size", size);
    formData.append("country", country);
    formData.append("city", city);
    formData.append("streetName", streetName);
    formData.append("postalCode", postalCode);
    formData.append("imageLink", imageLink.toString());

    try {
      const response = await axios.post(
       `${config.apiBaseUrl}/room`,
        
        formData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Room was listed successfully!", response);
      toast("Room is listed successfully!", { type: "success" });
      navigate("/rooms");
    } catch (error) {
      console.error("There was an error uploading the room data!", error);
      toast("There was an error listing the room!", { type: "error" });
    }
  };

  return (
    <div>
      <div className="intro">
        <h1 className="Title-listing">List your room</h1>
        <br />
        <img src={Logo} className="Logo" alt="Logo MyRoomPal" />
      </div>

      <div className="Listening-info">
        <Row>
          <Col md={6}>
            <h2 className="sub">Room information</h2>
          </Col>
          <Col md={6}>
            <img src={Room} className="Room" alt="A room" />
          </Col>
        </Row>

        <Form
          method="post"
          noValidate
          validated={validated}
          onSubmit={handleSubmit}
        >
          <Row>
            <Col md={6}>
              <Form.Group className="mb-3 w-100" controlId="validationCustom00">
                <Form.Label>Room name</Form.Label>
                <Form.Control
                  required
                  type="text"
                  name="roomName"
                  value={roomName}
                  onChange={handleRoomNameChange}
                  placeholder="Lorem Ipsum"
                />
                <Form.Control.Feedback type="invalid">
                  Please provide a title.
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group className="mb-3 w-100" controlId="validationCustom01">
                <Form.Label>Description</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={5}
                  required
                  name="description"
                  value={description}
                  onChange={handleDescriptionChange}
                  placeholder="............."
                />
                <Form.Control.Feedback type="invalid">
                  Please provide a description.
                </Form.Control.Feedback>
              </Form.Group>

              <Row className="mb-3 w-100">
                <Form.Group as={Col} md="6" controlId="validationCustom02">
                  <Form.Label>Rent price</Form.Label>
                  <Form.Control
                    required
                    type="number"
                    name="rentPrice"
                    value={rentPrice}
                    onChange={handleRentPriceChange}
                    placeholder="€"
                  />
                  <Form.Control.Feedback type="invalid">
                    Please provide a rent price.
                  </Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} md="6" controlId="validationCustom03">
                  <Form.Label>Room size</Form.Label>
                  <Form.Control
                    required
                    type="number"
                    name="size"
                    value={size}
                    onChange={handleSizeChange}
                    placeholder="m2"
                  />
                  <Form.Control.Feedback type="invalid">
                    Please provide the size of the room in m2
                  </Form.Control.Feedback>
                </Form.Group>
              </Row>
            </Col>

            <Col md={6} className="upload-image">
              <Form.Group controlId="formFileMultiple" className="mb-3 w-100">
                <Form.Label>Add image(s)</Form.Label>
                <Form.Control
                  type="file"
                  name="images"
                  onChange={handleFileChange}
                  multiple
                  placeholder="Choose image(s)"
                  accept=".png, .jpg, .jpeg"
                  required
                />
                <Form.Control.Feedback type="invalid">
                  Please upload at least one image.
                </Form.Control.Feedback>
              </Form.Group>

              <Row className="mb-3 w-100">
                <Form.Group as={Col} md="6" controlId="validationCustom05">
                  <Form.Label>Street Address</Form.Label>
                  <Form.Control
                    required
                    type="text"
                    name="streetName"
                    value={streetName}
                    onChange={handleStreetNameChange}
                    placeholder="Street address"
                  />
                  <Form.Control.Feedback type="invalid">
                    Please provide the addres of the room
                  </Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} md="6" controlId="validationCustom06">
                  <Form.Label>City</Form.Label>
                  <Form.Control
                    required
                    type="text"
                    name="City"
                    value={city}
                    onChange={handleCityChange}
                    placeholder="City"
                  />
                  <Form.Control.Feedback type="invalid">
                    Please provide a city
                  </Form.Control.Feedback>
                </Form.Group>
              </Row>

              <Row className="mb-3 w-100">
                <Form.Group as={Col} md="6" controlId="validationCustom07">
                  <Form.Label>Country</Form.Label>
                  <Form.Control
                    required
                    type="text"
                    name="country"
                    value={country}
                    onChange={handleCountryChange}
                    placeholder="Country"
                  />
                  <Form.Control.Feedback type="invalid">
                    Please provide a country for the room
                  </Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} md="6" controlId="validationCustom08">
                  <Form.Label>Postal Code</Form.Label>
                  <Form.Control
                    required
                    type="text"
                    name="postalCode"
                    value={postalCode}
                    onChange={handlePostalCodeChange}
                    placeholder="Postal Code"
                  />
                  <Form.Control.Feedback type="invalid">
                    Please provide a postal code for the room
                  </Form.Control.Feedback>
                </Form.Group>
              </Row>
            </Col>
          </Row>
          <Button type="submit">Post Listing</Button>
        </Form>
      </div>

      <div className="Personal-info">
        <Row>
          <Col md={6}>
            <h2 className="sub">Personal information</h2>
          </Col>
          <Col md={6}>
            <img src={Person} className="Person" alt="A person" />
          </Col>
        </Row>
        <Form>
          <Row className="mb-3 w-100">
            <Form.Group as={Col} md="6">
              <Form.Label>First name</Form.Label>
              <Form.Control type="text" placeholder="John" />
            </Form.Group>
            <Form.Group as={Col} md="6">
              <Form.Label>Last name</Form.Label>
              <Form.Control type="text" placeholder="Doe" />
            </Form.Group>
          </Row>
          <Row className="mb-3 w-100">
            <Form.Group as={Col}>
              <Form.Label>Phone number</Form.Label>
              <Form.Control type="number" placeholder="+34669842926" />
            </Form.Group>
          </Row>
          <Row className="mb-3 w-100">
            <Form.Group as={Col} controlId="">
              <Form.Label>Street Address</Form.Label>
              <Form.Control type="text" placeholder="Street Address" />
            </Form.Group>
          </Row>
          <Row className="mb-3 w-100">
            <Form.Group as={Col} md="6">
              <Form.Label>State</Form.Label>
              <Form.Control type="text" placeholder="State" />
            </Form.Group>
            <Form.Group as={Col} md="6">
              <Form.Label>Postal Code</Form.Label>
              <Form.Control type="text" placeholder="Postal Code" />
            </Form.Group>
          </Row>
        </Form>
      </div>

      <div className="logo-container">
        <img src={NameLogo} className="NameLogo" alt="Name Logo MyRoomPal" />
      </div>
    </div>
  );
}
export default Listing;
