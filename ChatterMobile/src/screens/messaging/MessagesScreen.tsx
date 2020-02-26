import React, { useEffect } from 'react'
import { observer } from "mobx-react-lite";
import {GiftedChat} from 'react-native-gifted-chat'
import { useStore } from '../../store/store';

interface Props {
    otherUserId: number
}

export const MessagingScreen =  observer((props: Props) => {
    const store= useStore()
    useEffect(() => {
        store.messages.load(props.otherUserId)
    }, [])

    return (
        <GiftedChat 
            user={{_id: store.auth.userId}}
            messages={store.messages.all}
            />
    )
})

export default MessagingScreen