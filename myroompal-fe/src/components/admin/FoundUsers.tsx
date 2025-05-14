import UserTableRow from "./UserTableRow";
import { AdminUserSearchUser } from "../../models/AdminPanel/AdminUserSearchUser";
import EnumUserStatus from "../../models/EnumUserStatus";

interface FoundUsersProps {
    results?: AdminUserSearchUser[];
    searching: boolean;
    hasSearched: boolean;
    onStatusChange: (user: AdminUserSearchUser, newStatus: EnumUserStatus) => void;
    onDeleteUser: (user: AdminUserSearchUser) => void;
}

function FoundUsers({ results, searching, hasSearched, onStatusChange, onDeleteUser }: FoundUsersProps) {
    return (
        <div className="col-12 col-md-8 mt-4 mt-md-0 table-responsive">
            <h5>Found users</h5>
            <table className="table table-bordered">
                <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Phone</th>
                    <th>Status</th>
                    <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {searching ? (
                    <tr>
                        <td colSpan={6}>Searching...</td>
                    </tr>
                ) : !hasSearched ? (
                    <tr>
                        <td colSpan={6}>Please enter a search query</td>
                    </tr>
                ) : !results || results.length === 0 ? (
                    <tr>
                        <td colSpan={6}>No results found</td>
                    </tr>
                ) : (
                    results.map((user: AdminUserSearchUser) => (
                        <UserTableRow
                            key={user.id}
                            user={user}
                            onStatusChange={onStatusChange}
                            onDeleteUser={onDeleteUser}
                        />
                    ))
                )}
                </tbody>
            </table>
        </div>
    );
}

export default FoundUsers;
