import React, { useState } from 'react'
import { StyleSheet, TextInput, Text } from "react-native";
import { SafeAreaView } from 'react-native-safe-area-context';
import { TouchableOpacity } from 'react-native-gesture-handler';
import { StackNavigationProp } from '@react-navigation/stack';
import { AuthStackParamList } from '../../navigation/AuthStack';
import { useStore } from '../../store/store';

interface Props {
    navigation: StackNavigationProp<AuthStackParamList>
}

export const LoginScreen = (props: Props) => {

    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const store = useStore()

    const onSubmit = () => {
        store.auth.login(username, password)
    }

    return (
        <SafeAreaView style={styles.container}>
            <Text style={styles.title}>Chatter</Text>
            <TextInput style={styles.input} placeholder="Username" value={username} onChangeText={setUsername} />
            <TextInput style={styles.input} placeholder="Password" value={password} onChangeText={setPassword} />
            <TouchableOpacity style={styles.button} onPress={onSubmit} >
                <Text style={{color: 'white'}}>Login</Text>
            </TouchableOpacity>
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        marginVertical: 50,
    },
    title: {
        fontSize: 32,
        fontWeight: 'bold',
        fontFamily: ''
    },
    input: {
        backgroundColor: 'white',
        borderRadius: 10,
        padding: 10,
        width: 300,
        marginVertical: 5,
    },
    button: {
        alignItems: 'center',
        backgroundColor: '#204691',
        color: 'white',
        borderRadius: 10,
        paddingVertical: 15,
        width: 300,
        marginVertical: 10,
    }
})

export default LoginScreen