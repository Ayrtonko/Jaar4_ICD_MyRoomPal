import {ChangeEvent, FormEvent, useState} from "react";

interface UserSearchProps {
    setSearch?: (value: (((prevState: string) => string) | string)) => void
}

function UserSearch({setSearch}: UserSearchProps) {
    const [searchedValue, setSearchedValue] = useState<string>("")

    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (setSearch) {
            setSearch(searchedValue);
        }
    };

    return (
        <>
            <div className="col-12 col-md-4">
                <h3 className="fs-1">User search</h3>
                <form className="d-flex flex-column mt-5 gap-3" onSubmit={handleSubmit}>
                    <h4>Search</h4>
                    <input
                        type="text"
                        className="form-control w-75"
                        placeholder="ID, Name, Email"
                        aria-label="Search"
                        onChange={(event) => setSearchedValue(event.target.value)}
                        value={searchedValue}
                    />
                    <button type="submit" className="btn btn-primary w-25">Search</button>
                </form>
            </div>
        </>
    )
}

export default UserSearch;
