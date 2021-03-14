import React, { Component, useState } from 'react';
import { useNavigation } from '@react-navigation/native';
import {
    Container,
    InputArea,
    CustumButton,
    CustumButtonText,
    HeaderArea,
    HeaderTitle,
    Scroller
} from './styles';
import DateTimePickerModal from "react-native-modal-datetime-picker";
import SignInput from '../../components/SignInput';
import SearchIcon from '../../assets/search.svg';
import CarIcon from '../../assets/car.svg';
import {Text, View, Alert} from 'react-native';
import Api from '../../Api'
import moment from 'moment';

export default () => {

    const navigation = useNavigation();

    const [freguesiaNome, setFreguesia] = useState('');
    const [isDatePickerVisible, setDatePickerVisibility] = useState(false);
    const [inicio, setInicio] = useState('');
    const [fim, setFim] = useState('');
    const [chosenMode, setChosenMode] = useState(null);

    //calendar  example
    const showDatePicker = () => {
        setDatePickerVisibility(true);
    };
    
    const hideDatePicker = () => {
        setDatePickerVisibility(false);
    };

    const handleConfirm = (date) => {
        hideDatePicker();
        if(chosenMode){
            setInicio(moment(date).format('yyyy-MM-DDTHH:mm:ss'))
            Alert.alert(
                "Hora Selecionada!",
                inicio.toString(),
                [
                  {
                    text: "Cancel",
                    onPress: () => console.log("Cancel Pressed"),
                    style: "cancel"
                  },
                  { text: "OK", onPress: () => console.log("OK Pressed") }
                ],
                { cancelable: false }
              );
        }
        if(!chosenMode){
            setFim(moment(date).format('yyyy-MM-DDTHH:mm:ss'))
            Alert.alert(
                "Hora Selecionada!",
                fim.toString(),
                [
                  {
                    text: "Cancel",
                    onPress: () => console.log("Cancel Pressed"),
                    style: "cancel"
                  },
                  { text: "OK", onPress: () => console.log("OK Pressed") }
                ],
                { cancelable: false }
              );
          }
    };

    //back button
    const handleButton = async () => {
        let json = await Api.getReservas(freguesiaNome, inicio, fim);
                Alert.alert(
                    "Disponiveis: ",
                    JSON.stringify(json),
                    [
                      {
                        text: "Cancel",
                        onPress: () => console.log("Cancel Pressed"),
                        style: "cancel"
                      },
                      { text: "OK", onPress: () => console.log("OK Pressed") }
                    ],
                    { cancelable: false }
                );
    };
    

    return (
        <Container>
            <Scroller>
                <HeaderArea>
                    <HeaderTitle numberOfLines={2}>Estacione no seu parque favorito</HeaderTitle>
                </HeaderArea>
            </Scroller>
            <CarIcon width="100%" height="160" fill="#FFFFFF" />
            <InputArea>
                
                <SignInput
                    IconSvg={SearchIcon}
                    placeholder="Qual a Freguesia?"
                    value={freguesiaNome}
                    onChangeText={t => setFreguesia(t)}
                />

                <CustumButton 
                onPress={ ()=> {
                    setChosenMode(true);
                    showDatePicker();
                    }}>
                    <CustumButtonText>Data de Início</CustumButtonText>
                </CustumButton>
                
                <CustumButton onPress={ ()=> {
                    setChosenMode(false);
                    showDatePicker();
                    }}>
                    <CustumButtonText>Data de Fim</CustumButtonText>
                </CustumButton>
                <DateTimePickerModal
                    	isVisible={isDatePickerVisible}
                        mode="datetime"
                        onConfirm={handleConfirm}
                        onCancel={hideDatePicker}
                />
                <CustumButton onPress={handleButton}>
                    <CustumButtonText>PESQUISAR</CustumButtonText>
               
                </CustumButton>
            </InputArea>
        </Container>
    );
};



