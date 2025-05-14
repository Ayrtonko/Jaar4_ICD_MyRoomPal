import React, { useEffect, useState } from "react";
import { Card, Form, Row, Col, Button } from "react-bootstrap";
import { useAuth0 } from "@auth0/auth0-react"; // Auth0 React SDK
import axios from "axios";
import "./ProfileEdit.css";
import { config } from "config";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const ProfileEdit: React.FC = () => {
  const { getAccessTokenSilently } = useAuth0(); // Use Auth0 to get token
  const [userData, setUserData] = useState({
    Occupation: "",
    SchoolOrCompany: "",
    ReligiousPreferences: "",
    DietaryPreferences: "",
    SmokingHabits: false,
    SleepingHabits: "",
    SocialHabits: "",
    CleaningHabits: "",
  });

  const navigate = useNavigate();

  const [imagePreview, setImagePreview] = useState<string | ArrayBuffer | null>("");

  useEffect(() => {
    const loadUserData = async () => {
      try {
        const response = await axios.get(
          config.apiBaseUrl + "/profile"
        );
        setUserData(response.data);
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };
    loadUserData();
  }, [getAccessTokenSilently]);

  const handleChange = (e: React.ChangeEvent<HTMLElement>) => {
    const { name, value } = e.target as HTMLInputElement | HTMLSelectElement;
    setUserData((prevData) => ({
      ...prevData,
      [name]: value || "", // Provide fallback to empty string
    }));
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result);
        setUserData((prevData) => ({
          ...prevData,
          profilePicture: file.name,
        }));
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async () => {
    try {
      const token = await getAccessTokenSilently(); 
      const response = await axios.post(
        config.apiBaseUrl + "/profile",
        userData
      );
      if(response.status == 200){ 
        toast("profile is succesfully updated",{type: "success"});}
        navigate("/profile");
        } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("Error updating user data:", error.response?.data || error.message);
      } else {
        console.error("Unknown error:", error);
      }
    }
  };

  return (
    <section className="container mt-5">
      <Card>
        <Card.Body>
          <h2 className="text-center">Profile Edit</h2>
          {/* Added rows */}
          <Row>
            <Col xs={12} md={6}>
              <Form.Group controlId="school">
                <Form.Label>School/Company Name</Form.Label>
                <Form.Control
                  type="text"
                  name="SchoolOrCompany"
                  value={userData.SchoolOrCompany || ""}
                  onChange={handleChange}
                  placeholder="Enter School/Company Name"
                />
              </Form.Group>
            </Col>
            <Col xs={12} md={6}>
              <Form.Group controlId="Occupation">
                <Form.Label>Occupation</Form.Label>
                <Form.Control
                  type="text"
                  name="Occupation"
                  value={userData.Occupation || ""}
                  onChange={handleChange}
                  placeholder="Enter Occupation"
                />
              </Form.Group>
            </Col>
          </Row>

          <Row>
            <Col xs={12} md={6}>
              <Form.Group controlId="religion">
                <Form.Label>Religious Preference</Form.Label>
                <Form.Control
                  type="text"
                  name="ReligiousPreferences"
                  value={userData.ReligiousPreferences || ""}
                  onChange={handleChange}
                  placeholder="Enter Religious Preference"
                />
              </Form.Group>
            </Col>
            <Col xs={12} md={6}>
              <Form.Group controlId="dietaryPreferences">
                <Form.Label>Dietary Preferences</Form.Label>
                <Form.Control
                  as="select"
                  name="DietaryPreferences"
                  value={userData.DietaryPreferences || ""}
                  onChange={handleChange}
                  className="custom-select"
                >
                  <option value="">Select Dietary Preference</option>
                  <option value="Vegetarian">Vegetarian</option>
                  <option value="Vegan">Vegan</option>
                  <option value="None">None</option>
                </Form.Control>
              </Form.Group>
            </Col>
          </Row>

          <Row>
            <Col xs={12} md={6}>
            <Form.Group controlId="smokingHabits">
  <Form.Label>Smoking Habits</Form.Label>
  <Form.Control
    as="select"
    name="SmokingHabits"
    value={userData.SmokingHabits ? "true" : "false"} // Convert boolean to string for select
    onChange={(e) => {
      const value = e.target.value === "true"; // Convert string to boolean
      setUserData((prevData) => ({
        ...prevData,
        SmokingHabits: value,
      }));
    }}
    className="custom-select"
  >
    <option value="true">Smoker</option>
    <option value="false">Non-Smoker</option>
  </Form.Control>
</Form.Group>


            </Col>
            <Col xs={12} md={6}>
              <Form.Group controlId="sleepingHabits">
                <Form.Label>Sleeping Habits</Form.Label>
                <Form.Control
                  as="select"
                  name="SleepingHabits"
                  value={userData.SleepingHabits || ""}
                  onChange={handleChange}
                  className="custom-select"
                >
                  <option value="">Select Sleeping Habit</option>
                  <option value="Early Bird">Early Bird</option>
                  <option value="Night Owl">Night Owl</option>
                </Form.Control>
              </Form.Group>
            </Col>
          </Row>

          {/* Social and Cleaning Habits */}
          <Row>
            <Col xs={12} md={6}>
              <Form.Group controlId="socialHabits">
                <Form.Label>Social Habits</Form.Label>
                <Form.Control
                  as="select"
                  name="SocialHabits"
                  value={userData.SocialHabits || ""}
                  onChange={handleChange}
                  className="custom-select"
                >
                  <option value="">Select Social Habit</option>
                  <option value="Introvert">Introvert</option>
                  <option value="Extrovert">Extrovert</option>
                </Form.Control>
              </Form.Group>
            </Col>
            <Col xs={12} md={6}>
              <Form.Group controlId="cleaningHabits">
                <Form.Label>Cleaning Habits</Form.Label>
                <Form.Control
                  as="select"
                  name="CleaningHabits"
                  value={userData.CleaningHabits || ""}
                  onChange={handleChange}
                  className="custom-select"
                >
                  <option value="">Select Cleaning Habit</option>
                  <option value="Messy">Messy</option>
                  <option value="Normal">Normal</option>
                  <option value="Neat">Neat</option>
                </Form.Control>
              </Form.Group>
            </Col>
          </Row>

          <Button variant="primary" type="submit" onClick={handleSubmit} className="mt-3">
            Save Changes
          </Button>
        </Card.Body>
      </Card>
    </section>
  );
};

export default ProfileEdit;
