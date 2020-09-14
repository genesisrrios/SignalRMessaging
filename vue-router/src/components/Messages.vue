<template>
  <div class="content">
    <div class="messages">
      <ul>
        <li class="sent">
          <p>What are you talking about? You do what they say or they shoot you.</p>
        </li>
        <li class="replies">
          <p>Wrong. You take the gun, or you pull out a bigger one. Or, you call their bluff. Or, you do any one of a hundred and forty six other things.</p>
        </li>
      </ul>
    </div>
    <div class="message-input">
      <div class="wrap">
      <input type="text" placeholder="Write your message..." />
      <i class="fa fa-paperclip attachment" aria-hidden="true"></i>
      <button class="submit"><i class="fa fa-paper-plane" aria-hidden="true"></i></button>
      </div>
    </div>
  </div>
</template>

<script>
import mixins from '../mixins.js'
import axios from 'axios'

export default {
  name: 'home',
  mixins: [mixins],
  data () {
    return {
    }
  },
  methods: {
  },
  created: function () {
    
  },
  mounted: function () {
    let messageBubbles = document.getElementsByClassName('sent')
    for (let i = 0; i < messageBubbles.length; i++) {
      messageBubbles[i].style.backgroundColor = localStorage.primaryColor
      messageBubbles[i].style.borderRadius = '20px'
    }
  }
}

</script>

<style scoped>
#frame .content .messages ul li.sent img {
  margin: 6px 8px 0 0;
}
#frame .content .messages ul li.sent p {
  color: #f5f5f5;
}
</style>
