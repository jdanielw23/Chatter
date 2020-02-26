import React, { useEffect } from 'react'
import { observer } from "mobx-react-lite";
import { View } from 'react-native';

interface Props {
    otherUserId: number
}

export const FriendsScreen =  observer((props: Props) => {
    return (
        <View></View>
    )
})

export default FriendsScreen