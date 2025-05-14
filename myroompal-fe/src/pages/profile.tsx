import React, { useEffect, useState } from 'react';
import img from '../Assets/user.webp';
import '../css/profile.css';
import { Link, useNavigate } from 'react-router-dom';
import { config } from '../config';
import axios from 'axios';
import { useAuth0 } from '@auth0/auth0-react';
import { toast } from 'react-toastify';

const Profile = () => {
  const { isAuthenticated, getAccessTokenSilently } = useAuth0();
  const [profile, setProfile] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserId = async () => {
      if(!isAuthenticated) return;

      try {
        const profileDetails = await axios.get(`${config.apiBaseUrl}/profile/`);
        const profileData = profileDetails.data;
        setProfile(profileDetails.data);

        const { profilePreferences } = profileData;
        const isEmpty = Object.values(profilePreferences).some(value => value === null || value === '');

        if(isEmpty) {
          toast("One or more preferences are empty. Please update your profile.", { type: "error" });
          navigate('/profile/edit');
        }
        setLoading(false);
      } catch (error) {
        console.error('Error fetching user id:', error);
        setLoading(false);
      }

    };
    fetchUserId();
  }, [isAuthenticated, getAccessTokenSilently, navigate]);

  return (
    <div className="profile">
        {loading ? <div>Loading...</div> :
            <>
      <div className="profile-header">
        <img src={img} alt="Profile" className="profile-pic" />
        <h2 className="full-name"> {profile.name}</h2>
      </div>
      <div className="profile-details">
        <p><strong>Age:</strong> {profile.age}</p>
        <p><strong>Location:</strong> {profile.location}</p>
        <p><strong>Email:</strong> {profile.email}</p>
        <p><strong>Phone number:</strong> {profile.phoneNumber}</p>
        <p><strong>Sleeping Habits:</strong> {profile.profilePreferences.sleepingHabits}</p>
        <p><strong>Social Habits:</strong> {profile.profilePreferences.socialHabits}</p>
        <p><strong>Cleaning Habits:</strong> {profile.profilePreferences.cleaningHabits}</p>
        <p><strong>Dietary preferences:</strong> {profile.profilePreferences.dietaryPreferences}</p>
        <p><strong>Occupation:</strong> {profile.profilePreferences.occupation}</p>
        <p><strong>School/Company Name:</strong> {profile.profilePreferences.schoolOrCompany}</p>
        <p><strong>Religious preferences:</strong> {profile.profilePreferences.religiousPreferences}</p>
        <p><strong>Smoking:</strong> {profile.profilePreferences.smokingHabits ? 'Yes' : 'No'}</p>
      </div>
      <Link to="/profile/edit" className="edit-button">
            Edit information
      </Link>
            </>
        }
    </div>

  );
};

export default Profile;
