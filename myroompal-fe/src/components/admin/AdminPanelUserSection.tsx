import UserSearch from "./UserSearch";
import FoundUsers from "./FoundUsers";
import { useEffect, useState } from "react";
import AdminPanelService from "../../services/AdminPanelService";
import { AdminUserSearchUser } from "../../models/AdminPanel/AdminUserSearchUser";
import EnumUserStatus from "../../models/EnumUserStatus";

function AdminPanelUserSection() {
    const [search, setSearch] = useState("");
    const [searchResults, setSearchResults] = useState<AdminUserSearchUser[]>([]);
    const [isSearching, setIsSearching] = useState<boolean>(false);
    const [hasSearched, setHasSearched] = useState<boolean>(false);

    useEffect(() => {
        const fetchData = async () => {
            if (search) {
                setIsSearching(true);
                const results = await AdminPanelService.getUsersFromSearch(search);
                if (results) setSearchResults(results);
                setIsSearching(false);
                setHasSearched(true);
            }
        };
        fetchData();
    }, [search]);

    const handleStatusChange = async (user: AdminUserSearchUser, newStatus: EnumUserStatus) => {
        await AdminPanelService.changeUserStatus(user, newStatus);
        setSearchResults(prevResults =>
            prevResults.map(u => (u.id === user.id ? { ...u, status: newStatus } : u))
        );
    };

    const handleDeleteUser = async (user: AdminUserSearchUser) => {
        await AdminPanelService.deleteUser(user);
        setSearchResults(prevResults => prevResults.filter(u => u.id !== user.id));
    };

    return (
        <div className="w-100 bg-light mt-5 vh-100">
            <div className="page-container py-4 w-full">
                <div className="row">
                    <UserSearch setSearch={setSearch} />
                    <FoundUsers
                        results={searchResults}
                        searching={isSearching}
                        hasSearched={hasSearched}
                        onStatusChange={handleStatusChange}
                        onDeleteUser={handleDeleteUser}
                    />
                </div>
            </div>
        </div>
    );
}

export default AdminPanelUserSection;
