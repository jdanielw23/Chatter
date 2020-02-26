import React from 'react'
import { observer } from 'mobx-react-lite'
import { createStackNavigator } from '@react-navigation/stack'
import MessagingScreen from '../screens/messaging/MessagesScreen'
import FriendsScreen from '../screens/messaging/FriendsScreen'

export type MessagesStackParamList = {
    Friends: undefined,
    Messages: {
        otherUserId: number
    },
}

const Stack = createStackNavigator<MessagesStackParamList>()
export default observer(() => (
    <Stack.Navigator >
        <Stack.Screen name="Friends" component={FriendsScreen} />
        <Stack.Screen name="Messages" component={MessagingScreen} />
    </Stack.Navigator>
))