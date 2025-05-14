import React, { useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import { useUserRole } from 'helpers/useUserRole';

interface ProtectedRouteProps {
    component: React.ComponentType<any>;
    requiredRole?: string;
}

const ProtectedRoute = ({ component: Component, requiredRole }: ProtectedRouteProps) => { 
    const { isAuthenticated, isLoading, loginWithRedirect } = useAuth0();
    const navigate = useNavigate();
    const location = useLocation();
    const userRole = useUserRole();

    useEffect(() => {
        if(isLoading) return;

        if(!isAuthenticated){
            if(location.pathname === '/match') {
                loginWithRedirect({
                    appState: { returnTo: '/match' }
                });
            } else {
            navigate('/', { replace: true });
            }
        }

        if(requiredRole && userRole !== null && userRole !== requiredRole){
            navigate('/', { replace: true });
        }

    }, [isAuthenticated, isLoading, navigate, requiredRole, userRole, location.pathname, loginWithRedirect]);

    if (isLoading || userRole == null) {
        return <div>Loading...</div>;
    }

    return isAuthenticated && (!requiredRole || userRole === requiredRole) ? <Component /> : null;
};

export default ProtectedRoute;
