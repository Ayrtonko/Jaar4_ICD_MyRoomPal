import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react';
import Topnav from './components/Topnav';
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import Profile from './pages/profile';
import Home from './pages/home';
import Match from './pages/match/match'
import {ToastContainer} from "react-toastify";
import AdminDashboard from "./pages/admin-dashboard/admin-dashboard";
import Rooms from './pages/Rooms';
import Footer from './components/Footer/Footer';
import ProtectedRoute from 'components/ProtectedRoute';
import { useAuth0 } from '@auth0/auth0-react';
import ProfileEdit from 'pages/ProfileEdit/ProfileEdit';
import { UserRoleProvider } from 'helpers/useUserRole';
import Listing from 'pages/Listing';
import RoomDetails from "./pages/RoomDetails";
import Report from "./pages/Report";

function App() {
    const { isLoading, error } = useAuth0();

    if (isLoading) return <div>Loading...</div>;
    if (error) return <div>Oops! {error.message}</div>;

    return (
        <div className="App">
            <UserRoleProvider>
                <BrowserRouter>
                    <Topnav/>
                    <Routes>
                        <Route path="/profile" element={<ProtectedRoute component={Profile}/>}/>
                        <Route path="/" element={<Home/>}/>
                        <Route path="/admin" element={<ProtectedRoute component={AdminDashboard} requiredRole="Moderator"/>} />
                        <Route path="/match" element={<ProtectedRoute component={Match}/>}/>
                        <Route path="/listing" element={<ProtectedRoute component={Listing}/>}/>
                        <Route path="/report" element={<ProtectedRoute component={Report}/>}/>
                        <Route path="/rooms" element={<Rooms/>}/>
                        <Route path="/profile-edit" element={<ProfileEdit />} />
                        <Route path="/room/:id" element={<RoomDetails />} />
                        <Route path="/profile/edit" element={<ProtectedRoute component={ProfileEdit}/>} />
                    </Routes>
                    <Footer/>
                </BrowserRouter>
            </UserRoleProvider>
            <ToastContainer/>
        </div>
    );
}

export default App;
