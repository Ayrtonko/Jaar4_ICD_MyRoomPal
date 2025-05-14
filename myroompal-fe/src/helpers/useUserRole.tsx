import {useState , useEffect, createContext, useContext} from 'react';
import {useAuth0} from '@auth0/auth0-react';
import axios from 'axios';
import {config} from '../config';

const UserRoleContext = createContext<string | null>(null);

export const UserRoleProvider = ({ children }: { children: React.ReactNode} ) => {
    const { isAuthenticated, getAccessTokenSilently } = useAuth0();
    const [userRole, setUserRole] = useState<string | null>(null);

    useEffect(() => {
        const fetchUserRole = async () => {
            if(!isAuthenticated) return;

            try {
                const token = await getAccessTokenSilently();
                const response = await axios.get(`${config.apiBaseUrl}/user/user-role`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                setUserRole(response.data.roleName);
            } catch (error) {
                console.error('Error fetching user role:', error);
            }
        };

        fetchUserRole();
    }, [isAuthenticated, getAccessTokenSilently]);

    return (
        <UserRoleContext.Provider value={userRole}>
            {children}
        </UserRoleContext.Provider>
    );
};

export const useUserRole = () => useContext(UserRoleContext);