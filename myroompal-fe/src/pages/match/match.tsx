import React, { useState} from 'react'
import "bootstrap-icons/font/bootstrap-icons.css";
import "../../css/match.scss"
import LocationSection from "../../components/Match/sections/LocationSection";
import PreferenceSection from "../../components/Match/sections/PreferenceSection";
import MatchSection from "../../components/Match/sections/MatchSection";

const Match = () => {
    const [locationValue, setLocationValue] = useState<string>('');
    const [selectedPreferences, setSelectedPreferences] = useState<string[]>([]);

    return (
        <>
            <LocationSection locationValue={locationValue} setLocationValue={setLocationValue}/>
            <PreferenceSection selectedPreferences={selectedPreferences}
                               setSelectedPreferences={setSelectedPreferences}/>
            <MatchSection locationValue={locationValue} selectedPreferences={selectedPreferences}  />
        </>
    );
};
export default Match;