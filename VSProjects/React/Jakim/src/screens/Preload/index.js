import React, { useEffect, useContext } from 'react';
import { Container, LoadingIcon } from './styles';
import { useNavigation } from '@react-navigation/native';
import { AsyncStorage } from 'react-native';
import {UserContext} from '../../contexts/UserContext';

//react native brings the async function pre built

//loading .svg
import CouveLogo from '../../assets/couve.svg';

export default () => {
    const {dispatch: userDispatch} = useContext(UserContext);
    const navigation = useNavigation();

    //evrytime the screen open executes code of checking login token
    //tries to get a token saved on device
    useEffect(() => {
        const checkToken = async () => {
            const token = await AsyncStorage.getItem('token');
            if (token) {
                //verify token
                navigation.reset({
                    routes:[{name: 'MainTab'}]
                }); 
            } else {
                navigation.navigate('SignIn');
            }
        }
        checkToken();
    }, []);

    return (
        <Container>
            <CouveLogo width="100%" height="160" />
            <LoadingIcon size= "large" color= "#FFFFFF"/>
        </Container>
    );
};