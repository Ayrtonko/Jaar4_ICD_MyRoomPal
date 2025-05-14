import React from "react";
import UserCard from "./UserCard";

import TProfileResponse from "../../models/Match/ProfileResponseType";

type TMatchListProps = {
    profiles: TProfileResponse[]
    matchBtn?: boolean
    chatBtn?: boolean
    deleteBtn?: boolean
    setDeleteToggle?: React.Dispatch<React.SetStateAction<boolean>>
    deleteToggle?: boolean
}

const MatchList = ({profiles, chatBtn, matchBtn, deleteBtn, deleteToggle, setDeleteToggle}: TMatchListProps) => {
    return (
        <>
            <div className="row mt-2 g-3">
                {profiles.length ? profiles
                    .sort((a, b) => b.matchScore - a.matchScore) // Sort profiles by matchScore in descending order
                    .map((profile: TProfileResponse) => (
                        <UserCard key={profile.userId}
                                  accountId={profile.userId}
                                  name={profile.firstName + " " + profile.lastName}
                                  email={profile.email}
                                  matchScore={profile.matchScore}
                                  chatBtn={chatBtn}
                                  matchBtn={matchBtn}
                                  deleteBtn={deleteBtn}
                                  deleteToggle={deleteToggle}
                                  setDeleteToggle={setDeleteToggle}
                        />
                    )) : <p className="ms-2" cy-data="0-results-found">0 results found.</p>}
            </div>
        </>
    )
        ;
};

export default MatchList;
