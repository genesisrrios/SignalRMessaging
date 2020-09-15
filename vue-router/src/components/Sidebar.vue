<template>
    <div id='sidepanel' v-bind:style="{'background-color':userPrimaryColor}">
    <div id='profile'>
        <div class='wrap'>
            <img id='profile-img' v-bind:src='userProfileImage' v-bind:style="{'background-color':userPrimaryColor}" class='online' alt='profile image'>
            <p>{{username}}</p>
            <i class='fa fa-chevron-down expand-button' aria-hidden='true'></i>
            <div id='status-options'>
                <ul>
                <li id='status-online' class='active'><span class='status-circle'></span> <p>Online</p></li>
                </ul>
            </div>
        </div>
    </div>
    <div id='search'>
        <label for=''><i class='fa fa-search' aria-hidden='true'></i></label>
        <input v-bind:style="{'background-color':userPrimaryColor}" type='text' placeholder='Search contacts...' />
    </div>
    <div id='contacts'>
        <ul v-for="contact in contactList" v-bind:key="contact.user_id">
            <li class='contact' v-on:click="onContactMessagesClick(contact.user_id)">
                <div class='wrap'>
                <div class='meta'>
                    <p class='name'>{{contact.user_name}}</p>
                    <p class='preview'>{{contact.last_message_excerpt}}</p>
                </div>
                </div>
            </li>
            <hr>
        </ul>
    </div>
    <div id='bottom-bar'>
      <button data-toggle="modal" data-target="#modalComponent" v-bind:style="{'background-color':userPrimaryColor}"><i class='fa fa-user-plus fa-fw' aria-hidden='true'></i> <span>Add contact</span></button>
    </div >
  <searchmodal v-bind:primaryColor="userPrimaryColor"></searchmodal>
  </div>
</template>

<script>
import mixins from '../mixins.js'
import axios from 'axios'
import searchmodal from '@/components/SearchModal.vue'
import { EventBus } from '../event-bus.js'

export default {
  name: 'sidebar',
  components: {
    searchmodal
  },
  mixins: [mixins],
  data () {
    return {
      username: null,
      userPrimaryColor: null,
      userProfileImage: null,
      contactList: []
    }
  },
  methods: {
    getUser: async function () {
      let self = this
      if (localStorage.username && localStorage.userId) {
        axios.get(`${this.apiUrl}api/user/getuser?user_id=${localStorage.userId}`)
          .then(function (response) {
            if (!response.data.values) {
              self.createNewUser()
            } else {
              self.assignValuesToUserInformation(response.data.values)
              self.getContactList()
            }
          }).catch(error => {
            console.log(error)
          })
      } else {
        self.createNewUser()
      }
    },
    assignValuesToUserInformation: function (parameters) {
      localStorage.userId = parameters.id
      localStorage.username = parameters.user_name
      localStorage.primaryColor = parameters.primary_color_hex
      this.username = parameters.user_name
      this.userPrimaryColor = parameters.primary_color_hex
      this.pickRandomProfileImage(parameters.icon)     
    },
    pickRandomProfileImage: function (imageName) {
      this.userProfileImage = require(`../assets/${imageName}`)
    },
    createNewUser: function () {
      let self = this
      // eslint-disable-next-line
      axios.get(`${this.apiUrl}api/user/getnewuser`)
        .then(function (response) {
          let data = response.data.values
          self.assignValuesToUserInformation(data)
        }).catch(error => {
          console.log(error)
        })
    },
    getContactList: function () {
      let self = this
        axios.get(`${this.apiUrl}api/contact/getContacts?user_id=${localStorage.userId}`)
          .then(function (response) {
            if (response.data.values) {
              self.contactList = response.data.values
            }
            else{

            }
          }).catch(error => {
            console.log(error)
          })
    },
    onContactMessagesClick: function (contactId) {
      EventBus.$emit('show-contact-messages',contactId);
    }
  },
  created: function () {    
    let self = this
    EventBus.$on('contact-added', function(){
      self.getContactList();
    })
  },
  mounted: function () {
    this.getUser()
  }
}
</script>

<style>
</style>
